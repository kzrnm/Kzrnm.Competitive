using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Kzrnm.Competitive.Analyzer.CreateOperator.Specified;

internal class CompareOperatorEnumerateMember : OperatorEnumerateMember
{
    internal CompareOperatorEnumerateMember(SemanticModel semanticModel, ITypeSymbol typeSymbol, AnalyzerConfig config) : base(semanticModel, typeSymbol, config) { }

    protected override SyntaxKind? GetSyntaxKind(IMethodSymbol symbol)
        => symbol switch
        {
            { Parameters.Length: not 2 } => null,
            { Name: "GreaterThan" } => SyntaxKind.GreaterThanExpression,
            { Name: "GreaterThanOrEqual" } => SyntaxKind.GreaterThanOrEqualExpression,
            { Name: "LessThan" } => SyntaxKind.LessThanExpression,
            { Name: "LessThanOrEqual" } => SyntaxKind.LessThanOrEqualExpression,
            _ => null,
        };
}
