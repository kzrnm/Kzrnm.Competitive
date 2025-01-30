using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Immutable;

namespace Kzrnm.Competitive.Analyzer.IntToLong;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class Analyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        => [DiagnosticDescriptors.KZCOMPETITIVE0001_OverflowInt32_Descriptor];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeIntToLongSyntaxNode,
            SyntaxKind.LeftShiftExpression, SyntaxKind.MultiplyExpression);
    }

    private void AnalyzeIntToLongSyntaxNode(SyntaxNodeAnalysisContext context)
    {
        var semanticModel = context.SemanticModel;
        var node = context.Node;

        var typeInfo = semanticModel.GetTypeInfo(node, cancellationToken: context.CancellationToken);
        if (typeInfo.Type.SpecialType is
            not SpecialType.System_Int32 and
            not SpecialType.System_UInt32)
            return;

        for (; node is not null; node = GetParent(node))
        {
            if (semanticModel.GetTypeInfo(node, cancellationToken: context.CancellationToken)
                .ConvertedType.SpecialType is var convertedType && convertedType is SpecialType.System_Int64 or SpecialType.System_UInt64)
            {
                var isUnsigned = convertedType is SpecialType.System_UInt64;
                var diagnostic = node.Kind() switch
                {
                    SyntaxKind.MultiplyExpression
                    or SyntaxKind.LeftShiftExpression => DiagnosticDescriptors.KZCOMPETITIVE0001_OverflowInt32(context.Node, isUnsigned),
                    _ => null,
                };

                if (diagnostic != null)
                    context.ReportDiagnostic(diagnostic);
                return;
            }
        }

        static SyntaxNode GetParent(SyntaxNode node)
        {
            var parent = node.Parent;
            if (parent is BinaryExpressionSyntax or ParenthesizedExpressionSyntax)
                return parent;
            return null;
        }
    }
}
