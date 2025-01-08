// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;

namespace AterraEngine.DependencyInjection.Generators.Sample;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// [InjectableService<ISecondDualExampleService>(ServiceLifetime.Singleton)]
[InjectableService<IFirstDualExampleService>(ServiceLifetime.Singleton)]
public class DualExampleService : IFirstDualExampleService, ISecondDualExampleService;

public interface IFirstDualExampleService;

public interface ISecondDualExampleService;
