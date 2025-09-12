// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.CodeOfChaos.Extensions.DependencyInjection;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Injectable<IExampleService>(ServiceLifetime.Singleton)]
[InjectableSingleton<IExampleService>()]
[InjectableScoped<IExampleService>()]
[InjectableTransient<IExampleService>()]
public class ExampleService {
    
}

public interface IExampleService {
    
}
