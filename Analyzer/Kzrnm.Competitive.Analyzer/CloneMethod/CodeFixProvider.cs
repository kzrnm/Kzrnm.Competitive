using Kzrnm.Competitive.Analyzer.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace Kzrnm.Competitive.Analyzer.CloneMethod;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CodeFixProvider)), Shared]
public class CodeFixProvider : Microsoft.CodeAnalysis.CodeFixes.CodeFixProvider
{
    private const string title = "Cast the result of Clone()";
    public override ImmutableArray<string> FixableDiagnosticIds
        => [DiagnosticDescriptors.KZCOMPETITIVE0005_CastClone_Descriptor.Id];

    public sealed override FixAllProvider GetFixAllProvider()
        => WellKnownFixAllProviders.BatchFixer;

    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var diagnostic = context.Diagnostics[0];
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        var action = CodeAction.Create(title: title,
           createChangedDocument: async c =>
           {
               var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);
               var node = (InvocationExpressionSyntax)root.FindNode(context.Span);
               var semanticModel = await context.Document.GetSemanticModelAsync(context.CancellationToken).ConfigureAwait(false);
               var caller = ((MemberAccessExpressionSyntax)node.Expression).Expression;
               var returnType = semanticModel.GetTypeInfo(caller, context.CancellationToken).Type;
               var newNode = SyntaxFactory.ParenthesizedExpression(
                   SyntaxFactory.CastExpression(returnType.ToTypeSyntax(semanticModel, node.GetLocation().SourceSpan.Start), node));
               return context.Document.WithSyntaxRoot(root.ReplaceNode(node, newNode));
           },
           equivalenceKey: title);
        context.RegisterCodeFix(action, diagnostic);
    }
}
