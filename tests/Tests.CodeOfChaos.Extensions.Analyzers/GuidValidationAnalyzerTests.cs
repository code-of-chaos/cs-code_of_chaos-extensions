// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.Analyzers;
using CodeOfChaos.Testing.TUnit;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace Tests.CodeOfChaos.Extensions.Analyzers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class GuidValidationAnalyzerTests {
    [Test]
    public async Task GuidValidationAnalyzer_ShouldNotSetDiagnostic() {
        // Arrange
        var runner = new RoslynCompilationRunner()
            .AddDocument("Test.cs", """
                namespace TestProject;

                public class TestClass {
                    public Guid GuidField = "abc37c26-ed0c-47af-8e0a-176d2658324c".ToGuid();
                }
                """)
            .AddDiagnosticAnalyzer<GuidValidationAnalyzer>();

        // Act
        CompilationWithAnalyzers compilation = await runner.GetCompilationWithAnalyzersAsync();
        ImmutableArray<Diagnostic> diagnostics = await compilation.GetAllDiagnosticsAsync();

        // Assert
        await Assert.That(diagnostics).DoesNotContainDiagnostic("CODE001");
    }

    [Test]
    public async Task GuidValidationAnalyzer_ShouldSetDiagnostic() {
        // Arrange
        var runner = new RoslynCompilationRunner()
            .AddDocument("Test.cs", """
                namespace TestProject;

                public class TestClass {
                    public Guid GuidField = "INVALID GUID".ToGuid();
                }
                """)
            .AddDiagnosticAnalyzer<GuidValidationAnalyzer>();

        // Act
        CompilationWithAnalyzers compilation = await runner.GetCompilationWithAnalyzersAsync();
        ImmutableArray<Diagnostic> diagnostics = await compilation.GetAllDiagnosticsAsync();

        // Assert
        await Assert.That(diagnostics).ContainsDiagnostic("CODE001");
        
    }
}
