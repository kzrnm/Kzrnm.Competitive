using Kzrnm.Competitive.Analyzer.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Kzrnm.Competitive.Analyzer.CreateOperator.Specified;

internal class MinMaxValueEnumerateMember : EnumerateMember
{
    internal MinMaxValueEnumerateMember(SemanticModel semanticModel, ITypeSymbol typeSymbol, AnalyzerConfig config) : base(semanticModel, typeSymbol, config) { }
    protected override PropertyDeclarationSyntax CreatePropertySyntax(IPropertySymbol symbol, bool isStatic)
    {
        if (symbol is { Name: "MaxValue" or "MinValue" })
        {
            var returnType = symbol.Type.ToTypeSyntax(SemanticModel, SemanticModel.SyntaxTree.Length - 1);
            var property = MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                returnType,
                IdentifierName(symbol.Name));

            return PropertyDeclaration(returnType, symbol.Name)
                .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                .WithExpressionBody(ArrowExpressionClause(property))
                .WithSemicolonToken(SyntaxHelpers.SemicolonToken);
        }
        return base.CreatePropertySyntax(symbol, isStatic);
    }
}
