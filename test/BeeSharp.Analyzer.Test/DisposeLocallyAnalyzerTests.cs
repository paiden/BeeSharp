using System;
using System.Linq;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using TestHelper;
using Xunit;

namespace BeeSharp.Analyzer.Test
{
    public class LockedAnalyzerUnitTest : CodeFixVerifier
    {

        //No diagnostics expected to show up
        [Fact]
        public void WhenInvocationOutsideBlockUsingStatement_DiagIsBS3001()
        {
            // Arrange
            var test = WithTestMethodBody("using(var x = new NotExisting()) { lockedInt.WriteAccess().Value = 3 };");

            // Assert
            VerifyCSharpDiagnostic(test, CreateResult("lockedInt.WriteAccess()")
                .WithLoc(new DiagnosticResultLocation("Test0.cs", 12, 48)));
        }

        [Fact]
        public void WhenInvocationInsideBlockUsingStatement_DiagIsFine()
        {
            // Arrange
            var test = WithTestMethodBody(@"
using(var ia = lockedInt.ReadAccess())
{
}");

            // Assert
            VerifyCSharpDiagnostic(test);
        }

        [Fact]
        public void GivenTwoUsings_WhenInvocationInFirstOfTwoUsingBlocks_DiagIsFine()
        {
            var test = WithTestMethodBody(@"
using(var ia = lockedInt.ReadAccess())
using(var is = new NonExistant())
{
}");

            VerifyCSharpDiagnostic(test);
        }

        [Fact]
        public void GivenStrictType_WhenInvocationInsideUsingDeclaration_DiagIsBS3002()
        {
            // Arrange
            var test = WithTestMethodBody(@"
using var ia = lockedInt.ReadAccess();
");

            // Assert
            VerifyCSharpDiagnostic(test, DiagnosticResult.Create(DisposeLocallyAnalyzer.BS3002, "lockedInt.ReadAccess()")
                .WithLoc(new DiagnosticResultLocation("Test0.cs", 14, 28)));
        }

        [Fact]
        public void GivenConstructedLocalDisposable_WhenNotInUsing_RiasesDiag3001()
        {
            // Arrange
            var test = @"
using BeeSharp;

namespace Test 
{
    public class LocalDisp : IDisposeLocally { public void Dispose() {} }

    public class TestClass
    {
        public void TestMethod() 
        {
            var t = new LocalDisp();
        }
    }
}";

            // assert
            // Assert
            VerifyCSharpDiagnostic(test, DiagnosticResult.Create(DisposeLocallyAnalyzer.BS3001, "new LocalDisp()")
                .WithLoc(new DiagnosticResultLocation("Test0.cs", 12, 21)));
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new BeeSharpAnalyzerCodeFixProvider();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new DisposeLocallyAnalyzer();
        }

        private static DiagnosticResult CreateResult(params object[] formatMessageArgs)
        {
            return DiagnosticResult.Create(DisposeLocallyAnalyzer.BS3001, formatMessageArgs);
        }

        private static string WithTestMethodBody(string body)
        {
            var lines = body.Split("\n");
            body = string.Join(Environment.NewLine, lines.Select(l => "            " + l));

            return $@"
using BeeSharp.Threading

namespace Test
{{
    public class TestClass
    {{
        private readonly Locked<int> lockedInt = new Locked<int>(1);

        public void TestM()
        {{
{body}
        }}
    }}
}}";
        }
    }
}
