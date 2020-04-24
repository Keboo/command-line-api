using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.CommandLine.Analyzer;
using System.CommandLine.Analyzer.Test.Helpers;
using Microsoft.CodeAnalysis.Text;
using CodeFixVerifier = System.CommandLine.Analyzer.Test.Verifiers.CodeFixVerifier;

namespace System.CommandLine.Analyzer.Test
{
    [TestClass]
    public class UnitTest : CodeFixVerifier
    {

        //No diagnostics expected to show up
        [TestMethod]
        public void TestMethod1()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public void TestMethod2()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
        }
    }";
            var expected = new DiagnosticResult
            {
                Id = "SystemCommandLineAnalyzer",
                Message = String.Format("Type name '{0}' contains lowercase letters", "TypeName"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 11, 15)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);

            var fixtest = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TYPENAME
        {   
        }
    }";
            VerifyCSharpFix(test, fixtest);
        }

        [TestMethod]
        public void CanDetectCommandHandlerWithSingleParameter()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class Program
        {
            public static void Main(string[] args)
            {
                var command = new Command(""command"");
                command.AddOption(
                    new Option(""--name"")
                    {
                        Argument = new Argument<string>()
                    });
                
                command.Handler = CommandHandler.Create<string>([|name2|] => { });
            }
        }
    }";
            MarkupTestFile.GetLineAndColumn(test, out string code, out int line, out int column);

            var expected = new DiagnosticResult
            {
                Id = "SystemCommandLineAnalyzer",
                Message = "Parameter 'name2' does not match option --name",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                        new DiagnosticResultLocation("Test0.cs", line, column)
                    }
            };

            


            VerifyCSharpDiagnostic(code, expected);
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new SystemCommandLineAnalyzerCodeFixProvider();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new SystemCommandLineAnalyzerAnalyzer();
        }
    }
}
