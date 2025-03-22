// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Immutable;

namespace CodeOfChaos.Extensions.Analyzers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class GuidValidationAnalyzer : DiagnosticAnalyzer {
    private static readonly DiagnosticDescriptor Rule = new(
        "CODE001",
        "Invalid GUID in .ToGuid() call",
        "The string '{0}' is not a valid GUID and will raise an exception",
        "Usage",
        DiagnosticSeverity.Error,
        true,
        "Checks if the string used with the ToGuid() method is a valid GUID.");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule];

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Initialize(AnalysisContext context) {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.InvocationExpression);
    }

    private void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context) {
        if (context.Node is not InvocationExpressionSyntax invocation)
            return;

        if (invocation.Expression is not MemberAccessExpressionSyntax memberAccess
            || memberAccess.Name.Identifier.Text != "ToGuid")
            return;

        if (memberAccess.Expression is not LiteralExpressionSyntax literalExpression
            || literalExpression.Kind() != SyntaxKind.StringLiteralExpression)
            return;

        string guidString = literalExpression.Token.ValueText;
        if (Guid.TryParse(guidString, out _)) return;// If the string is a valid GUID, we don't need to report a diagnostic.

        var diagnostic = Diagnostic.Create(Rule, literalExpression.GetLocation(), guidString);
        context.ReportDiagnostic(diagnostic);
    }
}
