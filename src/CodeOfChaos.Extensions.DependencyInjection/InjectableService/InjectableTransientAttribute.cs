// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Extensions.DependencyInjection;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class InjectableTransientAttribute<TService>(string? key = null) : Attribute {
    public Type ServiceType { get; } = typeof(TService);
    public string? Key { get; } = key;
}
