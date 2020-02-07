extern alias bs;

using System;
using System.Linq;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using TestHelper;
using Xunit;


namespace BeeSharp.Analyzer.Test
{
    public class PreventDefaultConstructionTests : CodeFixVerifier
    {
        const int BodyFirstLine = 16;

        [Fact]
        public void GivenDefultConstructorInvocationOnNonDefualtConstructable_DiagIsBS3003()
        {
            // Arrange
            var test = WithTestMethodBody("var x = new NoDefConst();");

            // Assert
            VerifyCSharpDiagnostic(test, CreateResult("new NoDefConst()", "NoDefConst")
                .WithLoc(new DiagnosticResultLocation("Test0.cs", BodyFirstLine, 21)));
        }

        [Fact]
        public void GivenDefultInvocationOnNonDefualtConstructable_DiagIsBS3003()
        {
            // Arrange
            var test = WithTestMethodBody("var x = default(NoDefConst);");

            // Assert
            VerifyCSharpDiagnostic(test, CreateResult("default(NoDefConst)", "NoDefConst")
                .WithLoc(new DiagnosticResultLocation("Test0.cs", BodyFirstLine, 21)));
        }

        [Fact]
        public void GivenImplicitDefultInvocationOnNonDefualtConstructable_DiagIsBS3003()
        {
            // Arrange
            var test = WithTestMethodBody("NoDefConst x = default;");

            // Assert
            VerifyCSharpDiagnostic(test, CreateResult("default", "NoDefConst")
                .WithLoc(new DiagnosticResultLocation("Test0.cs", BodyFirstLine, 28)));
        }


        [Fact]
        public void GivenLinqFirstOrDefault_DiagIsBS3003()
        {
            // Arrange
            var test = WithTestMethodBody(@"
List<NoDefConst> l;
var x = l.TestOrDefault();");

            // Assert
            VerifyCSharpDiagnostic(test, CreateBS3004Result("l.TestOrDefault()", "NoDefConst")
                .WithLoc(new DiagnosticResultLocation("Test0.cs", BodyFirstLine + 4, 21)));
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return null;
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
            => new PreventDefaultConstructionAnalyzer();

        private static DiagnosticResult CreateResult(params object[] formatMessageArgs)
        {
            return DiagnosticResult.Create(PreventDefaultConstructionAnalyzer.BS3003, formatMessageArgs);
        }

        private static DiagnosticResult CreateBS3004Result(params object[] formatMessageArgs)
            => DiagnosticResult.Create(PreventDefaultConstructionAnalyzer.BS3004, formatMessageArgs);

        private static string WithTestMethodBody(string body)
        {
            var lines = body.Split("\n");
            body = string.Join(Environment.NewLine, lines.Select(l => "            " + l));

            return $@"
using System.Collections.Generic;
using System.Linq;
using BeeSharp;

namespace Test
{{
    public struct NoDefConst : {nameof(bs::BeeSharp.IPreventDefaultConstruction)}
    {{
    }}

    public class TestClass
    {{
        public void TestM()
        {{
{body}
        }}
    }}

    // For some reason I cannot get roslyn to accept real Linq in the test
    // Simulate a Linq invocation with our cusotm local extension method.
    public static class E
    {{
        public static T TestOrDefault<T>(this IEnumerable<T> e) => default(T);
    }}
}}";
        }
    }
}
