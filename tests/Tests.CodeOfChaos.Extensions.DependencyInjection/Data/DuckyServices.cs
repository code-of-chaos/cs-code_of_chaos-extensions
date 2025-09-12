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

public interface IDuckyService {
    string Quack(IDucky ducky);
}

public interface IKeyedDucky {
    int ChaoticFactor { get; }
}

[Injectable<IDuckyService>(ServiceLifetime.Singleton)]
public class DuckyService : IDuckyService {
    public string Quack(IDucky ducky) => ducky.QuackingNoise;
}

[InjectableTransient<IKeyedDucky>("viewer")]
public class ViewerKeyedDucky : IKeyedDucky {
    public int ChaoticFactor => 5;
}

[InjectableTransient<IKeyedDucky>("streamer")]
public class StreamerKeyedDucky : IKeyedDucky {
    public int ChaoticFactor => 10;
}
