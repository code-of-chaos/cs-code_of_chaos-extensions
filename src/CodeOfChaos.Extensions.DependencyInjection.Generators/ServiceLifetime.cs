// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Extensions.DependencyInjection.Generators;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public enum ServiceLifetime {
    Unknown = -1,
    Singleton = 0,
    Scoped = 1,
    Transient = 2,
}

public static class ServiceLifetimeUtlities {
    public static string ToFriendlyString(this ServiceLifetime lifetime) => lifetime switch {
        ServiceLifetime.Singleton => "Singleton",
        ServiceLifetime.Scoped => "Scoped",
        ServiceLifetime.Transient => "Transient",
        _ => "Unknown"
    };

    public static ServiceLifetime ToLifetime(int value) => (ServiceLifetime)(value % 3);
}
