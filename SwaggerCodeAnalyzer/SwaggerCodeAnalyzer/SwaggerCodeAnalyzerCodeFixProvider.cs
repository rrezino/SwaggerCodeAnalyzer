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
        private const string title = "Add SwaggerOperation attribute";
        private const string swaggerOperationAttributeName = "SwaggerOperation";
        private const string controllerSufix = "Controller";
        private const string defaultControllerName = "ControllerName_";

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

        private string GetAttributeValue(MethodDeclarationSyntax methodDeclaration)
        {
            if (methodDeclaration.Parent is ClassDeclarationSyntax)
            {
                var parentClass = methodDeclaration.Parent as ClassDeclarationSyntax;
                return @"""" + parentClass.Identifier.ValueText.Replace(controllerSufix, "") + "_" + methodDeclaration.Identifier.ValueText + @"""";
            }
            
            return @"""" + defaultControllerName + methodDeclaration.Identifier.ValueText + @"""";
        }

        private async Task<Solution> AddSwaggerOperationAttribute(Document document,
            MethodDeclarationSyntax methodDeclaration, CancellationToken cancellationToken)
        {
            var attributeValue = GetAttributeValue(methodDeclaration);

            var liralExpression = SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression,
                SyntaxFactory.Token(default(SyntaxTriviaList), SyntaxKind.StringLiteralToken, attributeValue,
                    attributeValue, default(SyntaxTriviaList)));

            var attributeArgument = SyntaxFactory.AttributeArgument(liralExpression);
            var attributeList = new SeparatedSyntaxList<AttributeArgumentSyntax>();

            attributeList = attributeList.Add(attributeArgument);
            var argumentList = SyntaxFactory.AttributeArgumentList(attributeList);


            var attributes = methodDeclaration.AttributeLists.Add(
                SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                        SyntaxFactory.Attribute(SyntaxFactory.IdentifierName(swaggerOperationAttributeName))
                            .WithArgumentList(argumentList)
                    )
                )
            );

            var root = await document.GetSyntaxRootAsync(cancellationToken);

            return document.WithSyntaxRoot(
                root.ReplaceNode(
                    methodDeclaration,
                    methodDeclaration.WithAttributeLists(attributes)
                )).Project.Solution;
        }
    }
}
