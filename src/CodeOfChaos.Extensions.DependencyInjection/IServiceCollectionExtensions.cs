// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class IServiceCollectionExtensions {
    #region AddServiceIfNotExists
    public static IServiceCollection AddServiceIfNotExists<TService, TImplementation>(
        this IServiceCollection services,
        ServiceLifetime lifetime
    )
        where TService : class
        where TImplementation : class, TService {
        if (services.FirstOrDefault(x => x.ServiceType == typeof(TService)) is not null) return services;

        switch (lifetime) {
            case ServiceLifetime.Singleton:
                services.AddSingleton<TService, TImplementation>();
                break;
            case ServiceLifetime.Scoped:
                services.AddScoped<TService, TImplementation>();
                break;
            case ServiceLifetime.Transient:
                services.AddTransient<TService, TImplementation>();
                break;
            default: throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null);
        }

        return services;
    }

    public static IServiceCollection AddSingletonIfNotExists<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService =>
        AddServiceIfNotExists<TService, TImplementation>(services, ServiceLifetime.Singleton);

    public static IServiceCollection AddScopedIfNotExists<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService =>
        AddServiceIfNotExists<TService, TImplementation>(services, ServiceLifetime.Scoped);

    public static IServiceCollection AddTransientIfNotExists<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService =>
        AddServiceIfNotExists<TService, TImplementation>(services, ServiceLifetime.Transient);
    #endregion
    
    #region AddKeyedServiceIfNotExists
    public static IServiceCollection AddKeyedServiceIfNotExists<TService, TImplementation>(
        this IServiceCollection services,
        object key,
        ServiceLifetime lifetime
    )
        where TService : class
        where TImplementation : class, TService {
        if (services.FirstOrDefault(x => x.ServiceType == typeof(TService) && x.ServiceKey == key) is not null)
            return services;

        switch (lifetime) {
            case ServiceLifetime.Singleton:
                services.AddKeyedSingleton<TService, TImplementation>(key);
                break;
            case ServiceLifetime.Scoped:
                services.AddKeyedScoped<TService, TImplementation>(key);
                break;
            case ServiceLifetime.Transient:
                services.AddKeyedTransient<TService, TImplementation>(key);
                break;
            default: throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null);
        }

        return services;
    }

    // Direct keyed lifetime overloads
    public static IServiceCollection AddKeyedSingletonIfNotExists<TService, TImplementation>(
        this IServiceCollection services,
        object key
    )
        where TService : class
        where TImplementation : class, TService =>
        AddKeyedServiceIfNotExists<TService, TImplementation>(services, key, ServiceLifetime.Singleton);

    public static IServiceCollection AddKeyedScopedIfNotExists<TService, TImplementation>(
        this IServiceCollection services,
        object key
    )
        where TService : class
        where TImplementation : class, TService =>
        AddKeyedServiceIfNotExists<TService, TImplementation>(services, key, ServiceLifetime.Scoped);

    public static IServiceCollection AddKeyedTransientIfNotExists<TService, TImplementation>(
        this IServiceCollection services,
        object key
    )
        where TService : class
        where TImplementation : class, TService =>
        AddKeyedServiceIfNotExists<TService, TImplementation>(services, key, ServiceLifetime.Transient);
    #endregion
}
