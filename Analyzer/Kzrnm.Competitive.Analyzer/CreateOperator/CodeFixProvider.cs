using Kzrnm.Competitive.Analyzer.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;

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

        if (root.FindNode(diagnosticSpan)
            is not GenericNameSyntax genericNode)
            return;

        var semanticModel = await context.Document.GetSemanticModelAsync(context.CancellationToken).ConfigureAwait(false);

        ImmutableArray<ITypeParameterSymbol> originalTypes;
        ImmutableArray<ITypeSymbol> writtenTypes;
        switch (semanticModel.GetSymbolInfo(genericNode, context.CancellationToken).Symbol)
        {
            case INamedTypeSymbol symbol:
                originalTypes = symbol.TypeParameters;
                writtenTypes = symbol.TypeArguments;
                break;
            case IMethodSymbol symbol:
                originalTypes = symbol.TypeParameters;
                writtenTypes = symbol.TypeArguments;
                break;
            default:
                return;
        }


        var writtenTypeSyntaxes = genericNode.TypeArgumentList.Arguments;

        if (originalTypes.Length != writtenTypes.Length)
            return;

        if (!OperatorTypesMatcher.TryParseTypes(semanticModel.Compilation, out var types))
            return;

        var defaultSet = ImmutableHashSet.Create<ITypeSymbol>(SymbolEqualityComparer.Default);
        var genericDicBuilder = ImmutableDictionary.CreateBuilder<ITypeParameterSymbol, ITypeSymbol>(SymbolEqualityComparer.Default);
        var constraintDicBuilder = ImmutableDictionary.CreateBuilder<string, ImmutableHashSet<ITypeSymbol>>();
        for (int i = 0; i < originalTypes.Length; i++)
        {
            var writtenTypeSyntax = writtenTypeSyntaxes[i];
            var originalType = originalTypes[i];
            var constraintTypes = originalType.ConstraintTypes;
            var writtenType = writtenTypes[i];

            if (!constraintTypes
                .OfType<INamedTypeSymbol>()
                .Select(ty => ty.ConstructedFrom)
                .Any(ty => types.IsMatch(ty)))
            {
                genericDicBuilder.Add(originalType, writtenType);
                continue;
            }

            if (writtenType.TypeKind == TypeKind.Error)
            {
                var name = writtenType.Name;
                var typeSymbols = constraintDicBuilder.GetValueOrDefault(name, defaultSet);
                constraintDicBuilder[name] = typeSymbols.Union(constraintTypes);
            }
        }
        if (constraintDicBuilder.Count == 0)
            return;

        var genericDic = genericDicBuilder.ToImmutable();
        var constraintArrayDic = ImmutableDictionary.CreateBuilder<string, ImmutableArray<ITypeSymbol>>();
        foreach (var p in constraintDicBuilder)
        {
            constraintArrayDic[p.Key]
                = p.Value.Select(sy => SymbolHelpers.ReplaceGenericType(sy, genericDic))
                .OrderBy(sy => sy.ToDisplayString())
                .ToImmutableArray();
        }

        var action = CodeAction.Create(title: title,
           createChangedDocument: c => new OperatorTypeSyntaxBuilder(semanticModel, config).AddOperatorType(
               context.Document,
               root,
               constraintArrayDic.ToImmutable()),
           equivalenceKey: title);
        context.RegisterCodeFix(action, diagnostic);
    }

    private class OperatorTypeSyntaxBuilder(SemanticModel semanticModel, AnalyzerConfig config)
    {
        private readonly int origPosition = semanticModel.SyntaxTree.Length;

        public async Task<Document> AddOperatorType(
            Document document,
            CompilationUnitSyntax root,
            ImmutableDictionary<string, ImmutableArray<ITypeSymbol>> constraintDic)
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


            var dec = DeclarationSyntax(operatorTypeName, semanticModel.SyntaxTree.Options switch
            {
                CSharpParseOptions parseOptions => parseOptions.LanguageVersion,
                _ => null,
            })
                .WithBaseList(SyntaxFactory.BaseList(
                    constraints.Select(c => (BaseTypeSyntax)SyntaxFactory.SimpleBaseType(c.ToTypeSyntax(semanticModel, origPosition))).ToSeparatedSyntaxList()))
                .WithMembers(SyntaxFactory.List(members.Distinct(MemberDeclarationEqualityComparer.Default)));
            return (dec, hasMethod);
        }
    }
}
