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
        => [DiagnosticDescriptors.KZCOMPETITIVE0004_DefineOperatorType_Descriptor];

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

        var notDefined = semanticModel.GetSymbolInfo(genericNode, context.CancellationToken) switch
        {
            { Symbol: INamedTypeSymbol symbol } => [types.EnumerateNotDefinedTypes(symbol)],
            { Symbol: IMethodSymbol symbol } => [types.EnumerateNotDefinedTypes(symbol)],
            { CandidateSymbols: { Length: > 0 } candidateSymbols } => candidateSymbols.Select(types.EnumerateNotDefinedTypes),
            _ => [],
        };
        var notDefinedTypes = notDefined.SelectMany(t => t.Select(s => s.WrittenType.Name)).Distinct().ToArray();

        if (notDefinedTypes.Length > 0)
        {
            var diagnostic = DiagnosticDescriptors.KZCOMPETITIVE0004_DefineOperatorType(genericNode.GetLocation(), notDefinedTypes);
            context.ReportDiagnostic(diagnostic);
        }
    }
}
