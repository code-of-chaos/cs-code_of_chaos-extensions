// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;

namespace CodeOfChaos.Extensions.DependencyInjection;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class KeyedInjectableServiceAttribute<TService>(string key, ServiceLifetime lifetime) : Attribute {
    public string Key { get; } = key;
    public ServiceLifetime Lifetime { get; } = lifetime;
    public Type ServiceType { get; } = typeof(TService);
}
