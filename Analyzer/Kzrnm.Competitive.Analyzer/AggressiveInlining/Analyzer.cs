using Kzrnm.Competitive.Analyzer.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Kzrnm.Competitive.Analyzer.AggressiveInlining;
using static Constants;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class Analyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        => ImmutableArray.Create(
            DiagnosticDescriptors.KZCOMPETITIVE0003_AgressiveInlining_Descriptor);

    private record ContainingOperatorTypes(INamedTypeSymbol MethodImplAttribute, INamedTypeSymbol IsOperatorAttribute)
    {
        public static bool TryParseTypes(Compilation compilation, out ContainingOperatorTypes types)
        {
            types = null;
            var methodImpl = compilation.GetTypeByMetadataName(System_Runtime_CompilerServices_MethodImplAttribute);
            if (methodImpl is null)
                return false;
            var isOperator = compilation.GetTypeByMetadataName(AtCoder_IsOperatorAttribute);
            if (isOperator is null)
                return false;
            types = new(methodImpl, isOperator);
            return true;
        }

        public bool HasIsOperatorAttribute(INamedTypeSymbol symbol)
        {
            foreach (var at in symbol.ConstructedFrom.GetAttributes())
                if (SymbolEqualityComparer.Default.Equals(at.AttributeClass, IsOperatorAttribute))
                    return true;
            return false;
        }

        public bool DoesNotHaveMethodImplInlining(IMethodSymbol m)
        {
            if (m.MethodKind is
                 not (MethodKind.ExplicitInterfaceImplementation or MethodKind.Ordinary)
                || m.IsImplicitlyDeclared)
                return false;

            // Skip ToString()
            if (m is { Name: nameof(object.ToString), Parameters.Length: 0 })
                return false;

            if (m.GetAttributes()
                .Where(at => SymbolEqualityComparer.Default.Equals(at.AttributeClass, MethodImplAttribute))
                .FirstOrDefault() is { } attr)
            {
                if (attr.ConstructorArguments is { Length: 0 })
                    return true;
                else
                {
                    var arg = attr.ConstructorArguments[0];
                    if (arg.Kind is TypedConstantKind.Primitive or TypedConstantKind.Enum)
                        try
                        {
                            return !((MethodImplOptions)Convert.ToInt32(arg.Value)).HasFlag(MethodImplOptions.AggressiveInlining);
                        }
                        catch
                        {
                            return true;
                        }
                }
            }
            return true;
        }
    }
    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.RegisterCompilationStartAction(compilationStartContext =>
        {
            if (ContainingOperatorTypes.TryParseTypes(compilationStartContext.Compilation, out var types))
            {
                compilationStartContext.RegisterSyntaxNodeAction(
                    c => AnalyzeTypeDecra(c, types),
                    SyntaxKind.StructDeclaration
                    , SyntaxKind.ClassDeclaration
                    , SyntaxKind.RecordDeclaration
                    , SyntaxKind.RecordStructDeclaration);
            }
        });
    }

    private void AnalyzeTypeDecra(SyntaxNodeAnalysisContext context, ContainingOperatorTypes types)
    {
        if (context.SemanticModel.GetDeclaredSymbol(context.Node, context.CancellationToken)
            is not INamedTypeSymbol symbol)
            return;
        var concurrentBuild = context.Compilation.Options.ConcurrentBuild;

        if (concurrentBuild)
        {
            if (!symbol.AllInterfaces
                .AsParallel(context.CancellationToken)
                .Any(types.HasIsOperatorAttribute))
                return;
        }
        else
        {
            if (!symbol.AllInterfaces
                .Do(_ => context.CancellationToken.ThrowIfCancellationRequested())
                .Any(types.HasIsOperatorAttribute))
                return;
        }

        string[] notMethodImplInliningMethods;
        if (concurrentBuild)
            notMethodImplInliningMethods = symbol.GetMembers()
                .AsParallel(context.CancellationToken)
                .OfType<IMethodSymbol>()
                .Where(types.DoesNotHaveMethodImplInlining)
                .Select(m => m.Name)
                .ToArray();
        else
            notMethodImplInliningMethods = symbol.GetMembers()
                .Do(_ => context.CancellationToken.ThrowIfCancellationRequested())
                .OfType<IMethodSymbol>()
                .Where(types.DoesNotHaveMethodImplInlining)
                .Select(m => m.Name)
                .ToArray();
        if (notMethodImplInliningMethods.Length == 0)
            return;

        var diagnostic = DiagnosticDescriptors.KZCOMPETITIVE0003_AgressiveInlining(
            context.Node.GetLocation(), notMethodImplInliningMethods);
        context.ReportDiagnostic(diagnostic);
    }
}
