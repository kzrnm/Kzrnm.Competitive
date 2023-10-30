using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Kzrnm.Competitive.Analyzer.CreateOperator.Specified;

internal class AdditionEnumerateMember : OperatorEnumerateMember
{
    internal AdditionEnumerateMember(SemanticModel semanticModel, ITypeSymbol typeSymbol, AnalyzerConfig config) : base(semanticModel, typeSymbol, config) { }

    protected override SyntaxKind? GetSyntaxKind(IMethodSymbol symbol)
        => symbol switch
        {
            { Parameters.Length: not 2 } => null,
            { Name: "Add" } => SyntaxKind.AddExpression,
            _ => null,
        };
}
