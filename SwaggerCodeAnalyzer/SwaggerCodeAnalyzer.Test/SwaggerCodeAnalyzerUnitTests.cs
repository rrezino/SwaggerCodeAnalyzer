using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using TestHelper;
using SwaggerCodeAnalyzer;

namespace SwaggerCodeAnalyzer.Test
{
    [TestClass]
    public class SwaggerMissingOperationAttributeTests : CodeFixVerifier
    {
        //No diagnostics expected to show up
        [TestMethod]
        public void TestForNonFalsePositives()
        {
            var test = Properties.Resources.AfterCheckMissingSwaggerOperationAttribute.ToString(); ;

            VerifyCSharpDiagnostic(test);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public void TestIfAllErrorsAreIdentifiedAndCorrected()
        {
            var inputFile = Properties.Resources.BeforeCheckMissingSwaggerOperationAttribute.ToString();
            var outputFile = Properties.Resources.AfterCheckMissingSwaggerOperationAttribute.ToString();

            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = "SwaggerCodeAnalyzer",
                    Message = String.Format("Type name 'NoClassMethod' does not contain SwaggerOperation attribute", "TypeName"),
                    Severity = DiagnosticSeverity.Error,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 30, 21),
                        }
                },
                new DiagnosticResult
                {
                    Id = "SwaggerCodeAnalyzer",
                    Message = String.Format("Type name 'Tests' does not contain SwaggerOperation attribute", "TypeName"),
                    Severity = DiagnosticSeverity.Error,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 38, 25),
                        }
                },
                new DiagnosticResult
                {
                    Id = "SwaggerCodeAnalyzer",
                    Message = String.Format("Type name 'Tests' does not contain SwaggerOperation attribute", "TypeName"),
                    Severity = DiagnosticSeverity.Error,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 47, 25)
                        }
                }
            };

            VerifyCSharpDiagnostic(inputFile, expected);
            VerifyCSharpFix(inputFile, outputFile);
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new SwaggerCodeAnalyzerCodeFixProvider();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new SwaggerCodeAnalyzerAnalyzer();
        }
    }
}