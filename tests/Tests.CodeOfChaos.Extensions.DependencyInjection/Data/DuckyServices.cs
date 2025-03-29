// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.CodeOfChaos.Extensions.DependencyInjection.Data;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IDucky {
    string QuackingNoise { get; }
}

public interface IDuckyFactory : IFactoryService<IDucky>;

public interface IDuckyService {
    string Quack(IDucky ducky);
}

public interface IKeyedDucky {
    int ChaoticFactor { get; }
}

[InjectableService<IDuckyService>(ServiceLifetime.Singleton)]
public class DuckyService : IDuckyService {
    public string Quack(IDucky ducky) => ducky.QuackingNoise;
}

[InjectableService<IDuckyFactory>(ServiceLifetime.Singleton)]
public class DuckyFactory : IDuckyFactory {
    public IDucky Create() => new Ducky();
}

[FactoryCreatedService<IDuckyFactory, IDucky>(ServiceLifetime.Transient)]
public class Ducky : IDucky {
    public string QuackingNoise { get; } = "Quack Quack";
}

[KeyedInjectableService<IKeyedDucky>("viewer", ServiceLifetime.Transient)]
public class ViewerKeyedDucky : IKeyedDucky {
    public int ChaoticFactor => 5;
}

[KeyedInjectableService<IKeyedDucky>("streamer", ServiceLifetime.Transient)]
public class StreamerKeyedDucky : IKeyedDucky {
    public int ChaoticFactor => 10;
}
