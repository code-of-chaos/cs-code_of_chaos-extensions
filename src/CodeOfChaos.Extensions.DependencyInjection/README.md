# ⛓️‍💥 CodeOfChaos.Extensions.DependencyInjection ⛓️‍💥

CodeOfChaos.Extensions.DependencyInjection is a library that provides tools for automizing dependency injection in .NET
projects.
It includes source generators for automatic registration of services and attributes to facilitate DI.

## Features

- **Automatic Service Registration:** Use source generators to automatically register services in your project.
- **Custom Attributes:** Define custom attributes to specify the lifetime and other details for your services.

## Getting Started

### Prerequisites

- .NET 9.0 or later

### Installation

You can install `CodeOfChaos.Extensions.DependencyInjection` via NuGet Package Manager:

```bash
dotnet add package CodeOfChaos.Extensions.DependencyInjection
```

You can install `CodeOfChaos.Extensions.DependencyInjection.Generator` via NuGet Package Manager:

```bash
dotnet add package CodeOfChaos.Extensions.DependencyInjection.Generator
```

### Usage

#### Define Services with Attributes

You can use the provided attributes to define services:

- `InjectableService`: Simple attribute to register a class to the implementation which is inserted as a type generic.
    ```csharp    
    public interface IExampleService;
    
    [InjectableService<IExampleService>(ServiceLifetime.Singleton)]
    public class ExampleService : IExampleService {
        // ...
    }
    ```


- `FactoryCreatedService` : Marks the class as a service which creation depends on another injected service.
    - The factory service must implement `IFactoryService<>`
  ```csharp
  public interface ICreatedService;
  
  [FactoryCreatedService<IExampleFactory, ICreatedService>(ServiceLifetime.Transient)]
  public class CreatedService : ICreatedService;
  
  // The above service is something that is created by the Factory service
  
  [InjectableService<IExampleFactory>(ServiceLifetime.Singleton)]
  public class ExampleFactory : IExampleFactory {
      public ICreatedService Create() => new CreatedService();
  }
  
  public interface IExampleFactory : IFactoryService<ICreatedService>;
  ```


- `PooledInjectableService` : Marks the class as a poolable service. This library creates a class `AutoPoolableService`
  under which the class will be registered.
    - It uses `PooledInjectableServiceObjectPolicy` to create a policy.
    - The poolable service must implement `PooledInjectableServiceObjectPolicy`
  ```csharp
  public interface IExamplePooled : IManualPoolable; 
  
  [PooledInjectableService<IExamplePooled, ExamplePooled>]
  public class ExamplePooled : IExamplePooled {
      public bool Reset() => true;
  }
  ```

#### Generate Service Registrations

The source generator will automatically create the necessary registration code. C
all the generated registration method in your `Startup` or `Program` class:

```csharp
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.RegisterServicesFromYourAssemblyName();
```

## Contributing

Contributions are welcome! Please fork this repository and submit a pull request.