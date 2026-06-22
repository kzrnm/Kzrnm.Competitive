using Kzrnm.Competitive.Analyzer.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Kzrnm.Competitive.Analyzer.CreateOperator;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CodeFixProvider)), Shared]
public class CodeFixProvider : Microsoft.CodeAnalysis.CodeFixes.CodeFixProvider
{
    private const string title = "Create operator type";
    public override ImmutableArray<string> FixableDiagnosticIds
        => [DiagnosticDescriptors.KZCOMPETITIVE0004_DefineOperatorType_Descriptor.Id];
    public sealed override FixAllProvider GetFixAllProvider()
        => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        if (await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false)
            is not CompilationUnitSyntax root)
            return;
        var config = AnalyzerConfig.Parse(context.Document.Project.AnalyzerOptions.AnalyzerConfigOptionsProvider.GlobalOptions);
        var diagnostic = context.Diagnostics[0];
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        if (root.FindNode(diagnosticSpan) is not GenericNameSyntax genericNode)
            return;

        var semanticModel = await context.Document.GetSemanticModelAsync(context.CancellationToken).ConfigureAwait(false);

        if (!OperatorTypesMatcher.TryParseTypes(semanticModel.Compilation, out var types))
            return;

        var targetCandidates = semanticModel.GetSymbolInfo(genericNode, context.CancellationToken) switch
        {
            { Symbol: INamedTypeSymbol symbol } => [types.EnumerateTypeSymbols(symbol)],
            { Symbol: IMethodSymbol symbol } => [types.EnumerateTypeSymbols(symbol)],
            { CandidateSymbols: { Length: > 0 } candidateSymbols } => candidateSymbols.Select(types.EnumerateTypeSymbols).ToArray(),
            _ => [],
        };

        var defaultSet = ImmutableHashSet.Create<ITypeSymbol>(SymbolEqualityComparer.Default);

        ImmutableDictionary<string, ImmutableArray<ITypeSymbol>>
            BuildConstraintArrays((IEnumerable<GenericParameterSymbol>, IEnumerable<GenericParameterSymbol>) candidate)
        {
            var (definedTypes, notDefinedOperators) = candidate;
            var genericDicBuilder = ImmutableDictionary.CreateBuilder<ITypeParameterSymbol, ITypeSymbol>(SymbolEqualityComparer.Default);
            foreach (var (originalType, writtenType) in definedTypes)
            {
                genericDicBuilder[originalType] = writtenType;
            }

            var constraintDicBuilder = ImmutableDictionary.CreateBuilder<string, ImmutableHashSet<ITypeSymbol>>();
            foreach (var (originalType, writtenType) in notDefinedOperators)
            {
                var typeSymbols = constraintDicBuilder.GetValueOrDefault(writtenType.Name, defaultSet);
                constraintDicBuilder[writtenType.Name] = typeSymbols.Union(originalType.ConstraintTypes);
            }

            var constraintArrayDic = ImmutableDictionary.CreateBuilder<string, ImmutableArray<ITypeSymbol>>();
            if (constraintDicBuilder.Count > 0)
            {
                var genericDic = genericDicBuilder.ToImmutable();
                foreach (var p in constraintDicBuilder)
                {
                    constraintArrayDic[p.Key]
                        = [.. p.Value.Select(sy => SymbolHelpers.ReplaceGenericType(sy, genericDic)).OrderBy(sy => sy.ToDisplayString())];
                }
            }
            return constraintArrayDic.ToImmutable();
        }


        var operatorTypeSyntaxBuilder = new OperatorTypeSyntaxBuilder(semanticModel, config, context.Document, root);

        CodeAction ToAction(ImmutableDictionary<string, ImmutableArray<ITypeSymbol>> constraints)
        {
            var title = string.Join(", ", constraints.Select(p => $"{p.Key}:{string.Join(", ", p.Value.Select(s => s.ToMinimalDisplayString(semanticModel, diagnosticSpan.Start)))}"));
            return ToActionSingle(constraints, title);
        }
        CodeAction ToActionSingle(ImmutableDictionary<string, ImmutableArray<ITypeSymbol>> constraints, string title)
        {
            Task<Document> CreateChangedDocument(CancellationToken cancellationToken) =>
                operatorTypeSyntaxBuilder.AddOperatorType(constraints);

            return CodeAction.Create(createChangedDocument: CreateChangedDocument, title: title, equivalenceKey: title);
        }

        var constraintsDicts = targetCandidates.Select(BuildConstraintArrays)
            .Where(d => d.Count > 0)
            .Distinct(new ConstraintArraysEqualityComparer())
            .ToArray();

        if (constraintsDicts.Length == 0)
            return;
        else if (constraintsDicts.Length == 1)
            context.RegisterCodeFix(ToActionSingle(constraintsDicts[0], title), diagnostic);
        else
            context.RegisterCodeFix(CodeAction.Create(title: title, [.. constraintsDicts.Select(ToAction)], true), diagnostic);


    }

    private class ConstraintArraysEqualityComparer : IEqualityComparer<ImmutableDictionary<string, ImmutableArray<ITypeSymbol>>>
    {
        public bool Equals(ImmutableDictionary<string, ImmutableArray<ITypeSymbol>> x, ImmutableDictionary<string, ImmutableArray<ITypeSymbol>> y)
        {
            if (x.Count != y.Count)
                return false;

            foreach (var (key, constraints) in x)
            {
                if (!y.TryGetValue(key, out var other))
                    return false;

                if (!constraints.SequenceEqual<ISymbol>(other, SymbolEqualityComparer.Default))
                    return false;
            }

            return true;
        }

        public int GetHashCode(ImmutableDictionary<string, ImmutableArray<ITypeSymbol>> obj)
        {
            var hash = obj.Count.GetHashCode();
            foreach ((string key, ImmutableArray<ITypeSymbol> constraints) in obj)
            {
                hash = hash * 31 + key.GetHashCode();
                foreach (var c in constraints)
                    hash = hash * 31 + SymbolEqualityComparer.Default.GetHashCode(c);
            }
            return hash;
        }
    }

    private class OperatorTypeSyntaxBuilder(
        SemanticModel semanticModel,
        AnalyzerConfig config,
        Document document,
        CompilationUnitSyntax root)
    {
        private readonly int origPosition = semanticModel.SyntaxTree.Length;

        public async Task<Document> AddOperatorType(ImmutableDictionary<string, ImmutableArray<ITypeSymbol>> constraintDic)
        {
            bool hasMethod = false;
            var usings = root.Usings.ToNamespaceHashSet();

            TypeDeclarationSyntax[] newMembers = new TypeDeclarationSyntax[constraintDic.Count];
            foreach (var (p, i) in constraintDic.Select((p, i) => (p, i)))
            {
                bool m;
                (newMembers[i], m) = Build(p.Key, p.Value);
                hasMethod |= m;
            }

            return document.WithSyntaxRoot(root.AddMembers(newMembers));

        }

        private (TypeDeclarationSyntax syntax, bool hasMethod) Build(string operatorTypeName, ImmutableArray<ITypeSymbol> constraints)
        {
            bool hasMethod = false;
            var members = ImmutableArray.CreateBuilder<MemberDeclarationSyntax>();
            var added = ImmutableHashSet.CreateBuilder<ITypeSymbol>(SymbolEqualityComparer.Default);

            foreach (var constraint in constraints)
            {
                foreach (var baseType in constraint.AllInterfaces.Append(constraint))
                {
                    if (!added.Add(baseType))
                        continue;

                    foreach (var (member, isMethod) in EnumerateMember.Create(semanticModel, baseType, config).EnumerateMemberSyntax())
                    {
                        members.Add(member);
                        hasMethod |= isMethod;
                    }
                }
            }

            var dec = DeclarationSyntax(
                operatorTypeName, (semanticModel.SyntaxTree.Options as CSharpParseOptions)?.LanguageVersion)
                .WithBaseList(SyntaxFactory.BaseList(
                    constraints.Select(c => (BaseTypeSyntax)SyntaxFactory.SimpleBaseType(c.ToTypeSyntax(semanticModel, origPosition))).ToSeparatedSyntaxList()))
                .WithMembers(SyntaxFactory.List(members.Distinct(MemberDeclarationEqualityComparer.Default)));
            return (dec, hasMethod);


            static TypeDeclarationSyntax DeclarationSyntax(string operatorTypeName, LanguageVersion? languageVersion)
            {
                switch ((int?)languageVersion)
                {
                    case null:
                        goto default;
                    case >= (int)LanguageVersion.CSharp10:
                        return SyntaxFactory.RecordDeclaration(SyntaxKind.RecordStructDeclaration, SyntaxFactory.Token(SyntaxKind.RecordKeyword), operatorTypeName)
                            .WithClassOrStructKeyword(SyntaxFactory.Token(SyntaxKind.StructKeyword))
                            .WithModifiers(SyntaxTokenList.Create(SyntaxFactory.Token(SyntaxKind.ReadOnlyKeyword)))
                            .WithOpenBraceToken(SyntaxFactory.Token(SyntaxKind.OpenBraceToken))
                            .WithCloseBraceToken(SyntaxFactory.Token(SyntaxKind.CloseBraceToken));
                    case >= (int)LanguageVersion.CSharp7_2:
                        return SyntaxFactory.StructDeclaration(operatorTypeName)
                            .WithModifiers(SyntaxTokenList.Create(SyntaxFactory.Token(SyntaxKind.ReadOnlyKeyword)));
                    default:
                        return SyntaxFactory.StructDeclaration(operatorTypeName);
                }
            }
        }
    }
}
