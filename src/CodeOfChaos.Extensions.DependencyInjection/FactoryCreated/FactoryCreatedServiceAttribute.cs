// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;

namespace CodeOfChaos.Extensions.DependencyInjection;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class FactoryCreatedServiceAttribute<TFactory, TService>(ServiceLifetime lifetime) : Attribute
    where TFactory : IFactoryService<TService> {
    public ServiceLifetime Lifetime { get; } = lifetime;
    public Type FactoryType { get; } = typeof(TFactory);
    public Type ServiceType { get; } = typeof(TService);
}
