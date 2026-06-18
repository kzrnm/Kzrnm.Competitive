using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;

namespace Kzrnm.Competitive.Analyzer.IntToLong;

using static Constants;

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

        var modIntType = context.Compilation.GetTypeByMetadataName(AtCoder_IModInt);

        var typeInfo = semanticModel.GetTypeInfo(node, cancellationToken: context.CancellationToken);
        if (typeInfo.Type.SpecialType is
            not SpecialType.System_Int32 and
            not SpecialType.System_UInt32)
            return;

        var containsMultiply = false;
        for (; node is not null; node = GetParent(node))
        {
            containsMultiply |= node.Kind() is
                SyntaxKind.MultiplyExpression
                or SyntaxKind.LeftShiftExpression;

            bool mayBeOverflow = node.Kind() is SyntaxKind.MultiplyExpression
                    or SyntaxKind.LeftShiftExpression
                    or SyntaxKind.DivideExpression
                    or SyntaxKind.ModuloExpression
                    or SyntaxKind.RightShiftExpression;

            if (mayBeOverflow)
            {
                var convertedType = semanticModel.GetTypeInfo(node, cancellationToken: context.CancellationToken).ConvertedType;

                if (convertedType.SpecialType is SpecialType.System_Int64 or SpecialType.System_UInt64
                    || convertedType.SpecialType is SpecialType.None
                        && convertedType.AllInterfaces.Select(t => t.OriginalDefinition).Contains(modIntType, SymbolEqualityComparer.Default))
                {
                    var isUnsigned = typeInfo.Type.SpecialType is SpecialType.System_UInt32;
                    context.ReportDiagnostic(DiagnosticDescriptors.KZCOMPETITIVE0001_OverflowInt32(context.Node, isUnsigned));
                    return;
                }
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
