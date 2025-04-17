// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class IServiceCollectionExtensions {
    public static IServiceCollection AddServiceIfNotExists<TService, TImplementation>(
        this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TService : class
        where TImplementation : class, TService
    {
        if (services.FirstOrDefault(x => x.ServiceType == typeof(TService)) is not null) return services;

        switch (lifetime) {
            case ServiceLifetime.Singleton: services.AddSingleton<TService, TImplementation>();
                break;
            case ServiceLifetime.Scoped: services.AddScoped<TService, TImplementation>();
                break;
            case ServiceLifetime.Transient: services.AddTransient<TService, TImplementation>();
                break;
            default: throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null);
        }

        return services;
    }

}
