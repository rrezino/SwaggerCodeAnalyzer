using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;

namespace SwaggerCodeAnalyzer
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(SwaggerCodeAnalyzerCodeFixProvider)), Shared]
    public class SwaggerCodeAnalyzerCodeFixProvider : CodeFixProvider
    {
        private const string title = "Add SwaggerOperation Attribute";

        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(SwaggerCodeAnalyzerAnalyzer.DiagnosticId); }
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            // TODO: Replace the following code with your own analysis, generating a CodeAction for each fix to suggest
            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            // Find the type declaration identified by the diagnostic.
            var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf()
                .OfType<MethodDeclarationSyntax>().First();

            // Register a code action that will invoke the fix.
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: title,
                    createChangedSolution: c => AddSwaggerOperationAttribute(context.Document, declaration, c),
                    equivalenceKey: title),
                diagnostic);
        }

        private async Task<Solution> AddSwaggerOperationAttribute(Document document,
            MethodDeclarationSyntax methodDeclaration, CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken);

            var value = @"""" + "ControllerName_" + methodDeclaration.Identifier.ValueText + @"""";

            var liralExpression = SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression,
                SyntaxFactory.Token(default(SyntaxTriviaList), SyntaxKind.StringLiteralToken, value,
                    value, default(SyntaxTriviaList)));

            var attributeArgument = SyntaxFactory.AttributeArgument(liralExpression);

            var otherList = new SeparatedSyntaxList<AttributeArgumentSyntax>();
            otherList = otherList.Add(attributeArgument);
            var argumentList = SyntaxFactory.AttributeArgumentList(otherList);


            var attributes = methodDeclaration.AttributeLists.Add(
                SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                        SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("SwaggerOperation"))
                            .WithArgumentList(argumentList)
                    )
                )
            );

            return document.WithSyntaxRoot(
                root.ReplaceNode(
                    methodDeclaration,
                    methodDeclaration.WithAttributeLists(attributes)
                )).Project.Solution;
        }
    }
}
