using Kzrnm.Competitive.Analyzer.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Kzrnm.Competitive.Analyzer.CreateOperator;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class Analyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        => ImmutableArray.Create(
            DiagnosticDescriptors.KZCOMPETITIVE0004_DefineOperatorType_Descriptor);

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.RegisterCompilationStartAction(compilationStartContext =>
        {
            if (OperatorTypesMatcher.TryParseTypes(compilationStartContext.Compilation, out var types))
            {
                compilationStartContext.RegisterSyntaxNodeAction(
                    c => AnalyzeGenericNode(c, types), SyntaxKind.GenericName);
            }
        });
    }
    private void AnalyzeGenericNode(SyntaxNodeAnalysisContext context, OperatorTypesMatcher types)
    {
        var semanticModel = context.SemanticModel;
        if (context.Node is not GenericNameSyntax genericNode)
            return;

        if (genericNode.TypeArgumentList.Arguments.Any(sy => sy.IsKind(SyntaxKind.OmittedTypeArgument)))
            return;
        var concurrentBuild = context.Compilation.Options.ConcurrentBuild;

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

        if (originalTypes.Length == 0 || originalTypes.Length != writtenTypes.Length)
            return;

        var notDefinedTypes = new List<string>();
        for (int i = 0; i < originalTypes.Length; i++)
        {
            var originalType = originalTypes[i];
            var writtenType = writtenTypes[i];

            if (concurrentBuild)
            {
                if (!originalType.ConstraintTypes.OfType<INamedTypeSymbol>()
                      .AsParallel(context.CancellationToken)
                      .Any(s => types.IsMatch(s.ConstructedFrom)))
                    continue;
            }
            else
            {
                if (!originalType.ConstraintTypes.OfType<INamedTypeSymbol>()
                      .Do(_ => context.CancellationToken.ThrowIfCancellationRequested())
                      .Any(s => types.IsMatch(s.ConstructedFrom)))
                    continue;
            }

            if (writtenType.TypeKind == TypeKind.Error)
                notDefinedTypes.Add(writtenType.Name.ToString());
        }
        if (notDefinedTypes.Count == 0)
            return;

        var diagnostic = DiagnosticDescriptors.KZCOMPETITIVE0004_DefineOperatorType(
            genericNode.GetLocation(), notDefinedTypes);
        context.ReportDiagnostic(diagnostic);
    }
}
