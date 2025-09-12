// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace CodeOfChaos.Extensions.DependencyInjection.Generators;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class InjectableAnalyzer : DiagnosticAnalyzer {
    private static readonly DiagnosticDescriptor Rule001 = new(
        "COCDI001",
        "Service does not implement the correct Injectable service type",
        "The class '{0}' is not marked with injectable service type of {1}",
        "Injectable",
        DiagnosticSeverity.Warning,
        true
    );

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [
        Rule001
    ];

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Initialize(AnalysisContext context) {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);

        context.RegisterSyntaxNodeAction(AnalyzeClassDeclaration, SyntaxKind.ClassDeclaration);
    }

    private static void AnalyzeClassDeclaration(SyntaxNodeAnalysisContext context) {
        foreach (InjectableData injectableData in InjectableData.FromSyntaxAnalyzer(context)) {
            if (context.Node is not ClassDeclarationSyntax classDeclaration) continue;
            if (context.SemanticModel.GetDeclaredSymbol(classDeclaration) is not {} classSymbol) continue;


            if (injectableData.ServiceTypeName == null) continue;

            INamedTypeSymbol? serviceTypeSymbol = context.SemanticModel.Compilation.GetTypeByMetadataName(injectableData.ServiceTypeName);

            if (serviceTypeSymbol is null) continue;
            if (DoesClassImplementOrInherit(classSymbol, serviceTypeSymbol)) continue;

            var diagnostic = Diagnostic.Create(
                Rule001,
                context.Node.GetLocation(),
                // injectableData.LocationInfo?.ToLocation(),
                injectableData.ClassName,
                injectableData.ServiceTypeName
            );
            context.ReportDiagnostic(diagnostic);

        }
    }
    
    private static bool DoesClassImplementOrInherit(INamedTypeSymbol classSymbol, INamedTypeSymbol serviceTypeSymbol) {
        if (SymbolEqualityComparer.Default.Equals(classSymbol, serviceTypeSymbol)) return true;

        INamedTypeSymbol? baseType = classSymbol.BaseType;
        while (baseType is not null) {
            if (SymbolEqualityComparer.Default.Equals(baseType, serviceTypeSymbol)) return true;
            baseType = baseType.BaseType;
        }

        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        foreach (INamedTypeSymbol? implementedInterface in classSymbol.AllInterfaces) {
            if (!SymbolEqualityComparer.Default.Equals(implementedInterface, serviceTypeSymbol)) continue;
            return true;
        }

        return false;
    }
}
