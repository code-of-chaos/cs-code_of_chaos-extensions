// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.CodeOfChaos.Extensions.DependencyInjection;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Injectable<IExampleService>(ServiceLifetime.Singleton), InjectableSingleton<IExampleService>()]
[InjectableScoped<IExampleService>]
[InjectableTransient<IExampleService>]
[InjectableTransient<IExampleService>("null")]
[InjectableTransient<IExampleService>(2)]
[InjectableTransient<IExampleService>(Something)]
public class ExampleService : IExampleService {
    private const string Something = "something";
}

public interface IExampleService {
    
}
