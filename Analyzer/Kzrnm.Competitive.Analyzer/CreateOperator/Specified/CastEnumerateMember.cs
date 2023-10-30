using Kzrnm.Competitive.Analyzer.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Kzrnm.Competitive.Analyzer.CreateOperator.Specified;

internal class CastEnumerateMember : EnumerateMember
{
    internal CastEnumerateMember(SemanticModel semanticModel, ITypeSymbol typeSymbol, AnalyzerConfig config) : base(semanticModel, typeSymbol, config) { }
    protected override MethodDeclarationSyntax CreateMethodSyntax(IMethodSymbol symbol, bool isStatic)
    {
        if (symbol is
            {
                Name: "Cast",
                Parameters.Length: 1
            })
        {
            return CreateMethodSyntax(
                SemanticModel, SemanticModel.SyntaxTree.Length - 1,
                symbol, isStatic,
                ArrowExpressionClause(CastExpression(symbol.ReturnType.ToTypeSyntax(SemanticModel, SemanticModel.SyntaxTree.Length - 1), IdentifierName(symbol.Parameters[0].Name))));
        }
        return base.CreateMethodSyntax(symbol, isStatic);
    }
}
