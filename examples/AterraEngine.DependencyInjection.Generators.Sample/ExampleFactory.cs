// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;

namespace AterraEngine.DependencyInjection.Generators.Sample;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[FactoryCreatedService<IExampleFactory, ICreatedService>(ServiceLifetime.Transient)]
public class CreatedService : ICreatedService;

public interface ICreatedService;

[InjectableService<IExampleFactory>(ServiceLifetime.Singleton)]
public class ExampleFactory : IExampleFactory {
    public ICreatedService Create() => new CreatedService();
}

public interface IExampleFactory : IFactoryService<ICreatedService>;
