// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class ServiceCollectionExtensions {
    
    public static IServiceCollection AddUnitOfWork<TDbContext>(this IServiceCollection services) where TDbContext : DbContext {
        services.AddScoped<IUnitOfWorkFactory<TDbContext>, UnitOfWorkFactory<TDbContext>>();
        services.AddScoped<IUnitOfWork<TDbContext>>(static sp => sp.GetRequiredService<IUnitOfWorkFactory<TDbContext>>().Create());

        return services;
    }

    public static IServiceCollection AddUnitOfWork<TDbContext>(this IServiceCollection services, string key) where TDbContext : DbContext {
        services.AddKeyedScoped<IUnitOfWorkFactory<TDbContext>, UnitOfWorkFactory<TDbContext>>(key);
        services.AddKeyedScoped<IUnitOfWork<TDbContext>>(key, implementationFactory: static (sp, k) => sp.GetRequiredKeyedService<IUnitOfWorkFactory<TDbContext>>(k).Create());

        return services;
    }

    public static IServiceCollection AddReadonlyUnitOfWork<TDbContext>(this IServiceCollection services) where TDbContext : DbContext, IReadonlyCapableDbContext {
        services.AddScoped<IReadonlyUnitOfWorkFactory<TDbContext>, ReadonlyUnitOfWorkFactory<TDbContext>>();
        services.AddScoped<IReadonlyUnitOfWork<TDbContext>>(static sp => sp.GetRequiredService<IReadonlyUnitOfWorkFactory<TDbContext>>().Create());
        
        return services;
    }

    public static IServiceCollection AddReadonlyUnitOfWork<TDbContext>(this IServiceCollection services, string key) where TDbContext : DbContext, IReadonlyCapableDbContext {
        services.AddKeyedScoped<IReadonlyUnitOfWorkFactory<TDbContext>, ReadonlyUnitOfWorkFactory<TDbContext>>(key);
        services.AddKeyedScoped<IReadonlyUnitOfWork<TDbContext>>(key, implementationFactory: static (sp, k) => sp.GetRequiredKeyedService<IReadonlyUnitOfWorkFactory<TDbContext>>(k).Create());
        
        return services;
    }
}
