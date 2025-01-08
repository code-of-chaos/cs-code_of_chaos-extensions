// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.DependencyInjection.Generators.Sample;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[PooledInjectableService<IExamplePooled, ExamplePooled>]
public class ExamplePooled : IExamplePooled {
    public bool Reset() => true;
}

public interface IExamplePooled : IManualPoolable;
