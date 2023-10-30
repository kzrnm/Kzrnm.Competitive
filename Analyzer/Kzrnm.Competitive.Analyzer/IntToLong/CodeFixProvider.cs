using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kzrnm.Competitive.Analyzer.IntToLong;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CodeFixProvider)), Shared]
public class CodeFixProvider : Microsoft.CodeAnalysis.CodeFixes.CodeFixProvider
{
    private const string title = "Cast 32 bit number to 64 bit number";
    public override ImmutableArray<string> FixableDiagnosticIds
        => ImmutableArray.Create(
            DiagnosticDescriptors.KZCOMPETITIVE0001_MultiplyOverflowInt32_Descriptor.Id,
            DiagnosticDescriptors.KZCOMPETITIVE0002_ShiftOverflowInt32_Descriptor.Id);

    public sealed override FixAllProvider GetFixAllProvider()
        => WellKnownFixAllProviders.BatchFixer;

    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        if (await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false)
            is not CompilationUnitSyntax root)
            return;
        var diagnostic = context.Diagnostics[0];
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        bool.TryParse(diagnostic.Properties.GetValueOrDefault("IsUnsigned"), out var isUnsigned);
        var caster = new Caster(isUnsigned);

        var node = root.FindNode(diagnosticSpan);
        foreach (var nn in node.ChildNodes().Prepend(node))
        {
            if (nn is BinaryExpressionSyntax b)
            {
                switch (b.Kind())
                {
                    case SyntaxKind.RightShiftExpression:
                    case SyntaxKind.LeftShiftExpression:
                    case SyntaxKind.MultiplyExpression:
                        var action = CodeAction.Create(title: title,
                           createChangedDocument: c => caster.CastLong(context.Document, b, c),
                           equivalenceKey: title);
                        context.RegisterCodeFix(action, diagnostic);
                        return;
                }
            }
        }
    }
    readonly record struct Caster(bool IsUnsigned)
    {
        readonly SyntaxKind KeywordKind => IsUnsigned ? SyntaxKind.ULongKeyword : SyntaxKind.LongKeyword;
        public async Task<Document> CastLong(Document document, BinaryExpressionSyntax syntax, CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            while (syntax.Left is BinaryExpressionSyntax nx)
                syntax = nx;

            return document.WithSyntaxRoot(root.ReplaceNode(syntax.Left, CastSyntax(syntax)));
        }

        private ExpressionSyntax CastSyntax(BinaryExpressionSyntax syntax)
        {
            if (syntax.Left is LiteralExpressionSyntax lx)
            {
                if (TryCastLiteral(lx.Token, out var literal))
                {
                    return SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, literal)
                        .WithLeadingTrivia(syntax.Left.GetLeadingTrivia())
                        .WithTrailingTrivia(syntax.Left.GetTrailingTrivia());
                }
            }

            return SyntaxFactory.CastExpression(
                SyntaxFactory.PredefinedType(SyntaxFactory.Token(KeywordKind)), syntax.Left.WithoutTrivia())
                    .WithLeadingTrivia(syntax.Left.GetLeadingTrivia())
                    .WithTrailingTrivia(syntax.Left.GetTrailingTrivia());
        }
        private bool TryCastLiteral(SyntaxToken token, out SyntaxToken result)
        {
            SyntaxToken? tmp;
            if (IsUnsigned)
            {
                tmp = token.Value switch
                {
                    int num => SyntaxFactory.Literal((ulong)num),
                    uint num => SyntaxFactory.Literal((ulong)num),
                    _ => default(SyntaxToken?),
                };
            }
            else
            {
                tmp = token.Value switch
                {
                    int num => SyntaxFactory.Literal((long)num),
                    uint num => SyntaxFactory.Literal((long)num),
                    _ => default(SyntaxToken?),
                };
            }
            if (tmp is { } tmp2)
            {
                result = tmp2;
                return true;
            }
            result = default;
            return false;
        }
    }
}
