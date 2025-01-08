// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.CodeAnalysis;
using System.Text;

namespace CodeOfChaos.Extensions.DependencyInjection.Generators.Registrations;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IServiceRegistration {
    public INamedTypeSymbol ServiceTypeName { get; }
    public INamedTypeSymbol ImplementationTypeName { get; }
    public string LifeTime { get; }

    public void FormatText(StringBuilder builder, string assemblyName);
}
