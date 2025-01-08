// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Extensions.DependencyInjection;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class PooledInjectableServiceAttribute<TService, TImplementation> : Attribute
    where TService : IManualPoolable
    where TImplementation : TService, new()// Technically the generator picks up the base class, but this is just for clarity and some easy type checking
{
    public Type ServiceType { get; } = typeof(TService);
    public Type ImplementationType { get; } = typeof(TImplementation);
}
