using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;

namespace Kzrnm.Competitive.Analyzer.CloneMethod;

using static Constants;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class Analyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        => [DiagnosticDescriptors.KZCOMPETITIVE0005_CastClone_Descriptor];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(AnalyzeMethodCall, SyntaxKind.InvocationExpression);
    }

    private void AnalyzeMethodCall(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is InvocationExpressionSyntax
            {
                ArgumentList.Arguments.Count: 0,
                Parent.RawKind: not (int)SyntaxKind.CastExpression and not (int)SyntaxKind.AsExpression,
                Expression: MemberAccessExpressionSyntax
                {
                    Name: IdentifierNameSyntax { Identifier.Text: "Clone" }
                } expression
            } && context.SemanticModel.GetSymbolInfo(expression, context.CancellationToken).Symbol is IMethodSymbol
            {
                ReceiverType.Name: not "ICloneable",
                ReturnType.SpecialType: SpecialType.System_Object
            })
        {
            context.ReportDiagnostic(DiagnosticDescriptors.KZCOMPETITIVE0005_CastClone(context.Node.GetLocation()));
        }
    }
}
