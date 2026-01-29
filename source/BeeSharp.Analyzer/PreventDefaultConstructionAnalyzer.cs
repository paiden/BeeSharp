using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace BeeSharp.Analyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class PreventDefaultConstructionAnalyzer : BeeSharpAnalyzer
    {
        public static readonly DiagnosticDescriptor BS3003 = new DiagnosticDescriptor(
            nameof(BS3003),
            LocS(nameof(Resources.BS3003Title)),
            LocS(nameof(Resources.BS3003Message2A)),
            AnalyzerCategory.Default,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor BS3004 = new DiagnosticDescriptor(
            nameof(BS3004),
            LocS(nameof(Resources.BS3004Title)),
            LocS(nameof(Resources.BS3004Message2A)),
            AnalyzerCategory.Default,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; }
            = ImmutableArray.Create(BS3003, BS3004);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeConstructionExpression, SyntaxKind.ObjectCreationExpression);
            context.RegisterSyntaxNodeAction(AnalyzeDefaultExpression, SyntaxKind.DefaultExpression);
            context.RegisterSyntaxNodeAction(AnalyzeDefaultLiteralExpression, SyntaxKind.DefaultLiteralExpression);
        }

        private static void AnalyzeConstructionExpression(SyntaxNodeAnalysisContext context)
        {
            var expr = (ObjectCreationExpressionSyntax)context.Node;

            var ti = context.SemanticModel.GetTypeInfo(expr);
            if (!IsPreventDefaultConstructionType(ti))
            {
                return;
            }

            if (expr.ArgumentList.Arguments.Count <= 0)
            {
                ReportBS3003(context, expr, ti);
                return;
            }
        }

        private static void AnalyzeDefaultExpression(SyntaxNodeAnalysisContext context)
        {
            var expr = (DefaultExpressionSyntax)context.Node;

            var ti = context.SemanticModel.GetTypeInfo(expr);

            if (IsPreventDefaultConstructionType(ti))
            {
                ReportBS3003(context, expr, ti);
                return;
            }
        }

        private static void AnalyzeDefaultLiteralExpression(SyntaxNodeAnalysisContext context)
        {
            var expr = (LiteralExpressionSyntax)context.Node;

            var ti = context.SemanticModel.GetTypeInfo(expr);

            if (IsPreventDefaultConstructionType(ti))
            {
                ReportBS3003(context, expr, ti);
                return;
            }
        }

        private static void ReportBS3003(SyntaxNodeAnalysisContext context, SyntaxNode node, TypeInfo ti)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                BS3003, node.GetLocation(), node.ToString(), (ti.Type ?? ti.ConvertedType).Name));
        }

        private static void ReportBS3004(SyntaxNodeAnalysisContext context, SyntaxNode node, TypeInfo ti)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                BS3004, node.GetLocation(), node.ToString(), (ti.Type ?? ti.ConvertedType).Name));
        }

        private static bool IsPreventDefaultConstructionType(TypeInfo ti)
            => (ti.Type ?? ti.ConvertedType).Interfaces.Any(i => i.Name == nameof(IPreventDefaultConstruction));
    }
}
