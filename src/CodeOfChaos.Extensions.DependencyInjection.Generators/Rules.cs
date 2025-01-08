// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.CodeAnalysis;

namespace CodeOfChaos.Extensions.DependencyInjection.Generators;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class Rules {
    public static readonly DiagnosticDescriptor NoAttributesFound = new(
        "ATRDI001",
        "InjectableServiceAttribute not found",
        "InjectableServiceAttribute not found",
        "InjectableServices",
        DiagnosticSeverity.Info,
        true
    );

    public static readonly DiagnosticDescriptor NoAssemblyNameFound = new(
        "ATRDI002",
        "No assembly name was found",
        "No assembly name was found",
        "SourceGenerator",
        DiagnosticSeverity.Error,
        true
    );
}
