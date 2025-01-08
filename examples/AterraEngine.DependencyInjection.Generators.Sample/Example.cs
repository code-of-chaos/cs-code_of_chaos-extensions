// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;

namespace AterraEngine.DependencyInjection.Generators.Sample;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IExampleService>(ServiceLifetime.Singleton)]
public class ExampleService : IExampleService;

public interface IExampleService;
