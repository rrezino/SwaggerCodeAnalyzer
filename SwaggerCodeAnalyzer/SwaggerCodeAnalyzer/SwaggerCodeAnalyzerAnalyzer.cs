using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SwaggerCodeAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SwaggerCodeAnalyzerAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "SwaggerCodeAnalyzer";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "MissingAttribute";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var methodSymbol = (IMethodSymbol)context.Symbol;

            var attributes = methodSymbol.GetAttributes();

            var isRoute = attributes.Any(a => a.AttributeClass.Name.Contains("Route"));
            var isSwaggerEndpoint = attributes.Any(a => a.AttributeClass.Name.Contains("Swagger"));

            if (!(isRoute && isSwaggerEndpoint))
            {
                return;
            }

            var containsSwaggerOperation = attributes.Any(a => a.AttributeClass.Name.Contains("SwaggerOperation"));

            if (containsSwaggerOperation)
            {
                return;
            }

            var missingAttributeDiaginostic =
                Diagnostic.Create(Rule, methodSymbol.Locations[0], methodSymbol.Name);

            context.ReportDiagnostic(missingAttributeDiaginostic);
        }
    }
}
