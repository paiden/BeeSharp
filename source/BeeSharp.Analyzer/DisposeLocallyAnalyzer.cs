using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace BeeSharp.Analyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DisposeLocallyAnalyzer : DiagnosticAnalyzer
    {
        public static readonly DiagnosticDescriptor BS3002 = new DiagnosticDescriptor(
            nameof(BS3002),
            LocS(nameof(Resources.BS3002Title)),
            LocS(nameof(Resources.BS3002Message1A)),
            AnalyzerCategory.Default,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor BS3001 = new DiagnosticDescriptor(
            nameof(BS3001),
            LocS(nameof(Resources.BS3001Title)),
            LocS(nameof(Resources.BS3001Message1A)),
            AnalyzerCategory.Default,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; }
            = ImmutableArray.Create(BS3001, BS3002);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(Analyze, SyntaxKind.InvocationExpression);
        }

        private static void Analyze(SyntaxNodeAnalysisContext context)
        {
            var invocationSyntax = (InvocationExpressionSyntax)context.Node;

            var ti = context.SemanticModel.GetTypeInfo(invocationSyntax);
            if (!ti.Type.Interfaces.Any(i => i.Name == nameof(IDisposeLocallyStrict) || i.Name == nameof(IDisposeLocally)))
            {
                return;
            }

            var usingStatement = FindParentOf<UsingStatementSyntax>(invocationSyntax);
            if (usingStatement == null)
            {
                CreateDiagnostic(BS3001);
                return;
            }

            // We are not allowed to be inside the body of the using (we need to be inside the statement)
            var blockBetween = FindParentOf<BlockSyntax>(invocationSyntax, usingStatement);
            if (blockBetween != null)
            {
                CreateDiagnostic(BS3001);
                return;
            }

            if (IsStrict() && usingStatement.Statement is EmptyStatementSyntax) // check we are not inside new C#8's using declaration
            {
                CreateDiagnostic(BS3002);
                return;
            }

            bool IsStrict()
                => ti.Type.Interfaces.Any(i => i.Name == nameof(IDisposeLocallyStrict));

            void CreateDiagnostic(DiagnosticDescriptor desc)
            {
                context.ReportDiagnostic(Diagnostic.Create(desc, invocationSyntax.GetLocation(), invocationSyntax.ToString()));
            }
        }

        private static T FindParentOf<T>(SyntaxNode cur) where T : SyntaxNode
        {
            for (var p = cur.Parent; p != null; p = p.Parent)
            {
                if (p is T ofType)
                {
                    return ofType;
                }
            }

            return null;
        }

        private static T FindParentOf<T>(SyntaxNode cur, SyntaxNode stopHere) where T : SyntaxNode
        {
            for (var p = cur.Parent; p != null && p != stopHere; p = p.Parent)
            {
                if (p is T ofType)
                {
                    return ofType;
                }
            }

            return null;
        }

        private static LocalizableString LocS(string name)
            => new LocalizableResourceString(name, Resources.ResourceManager, typeof(Resources));
    }
}
