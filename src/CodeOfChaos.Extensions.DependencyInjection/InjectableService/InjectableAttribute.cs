// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace CodeOfChaos.Extensions.DependencyInjection;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class InjectableAttribute<T>(ServiceLifetime lifetime, object? key = null) : Attribute where T : class {
    [UsedImplicitly] public Type ServiceType => typeof(T);
    [UsedImplicitly] public ServiceLifetime Lifetime => lifetime;
    [UsedImplicitly] public object? Key => key;
}

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class InjectableSingletonAttribute<T>(object? key = null) : InjectableAttribute<T>(ServiceLifetime.Singleton, key) where T : class;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class InjectableScopedAttribute<T>(object? key = null) : InjectableAttribute<T>(ServiceLifetime.Scoped, key) where T : class;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class InjectableTransientAttribute<T>(object? key = null) : InjectableAttribute<T>(ServiceLifetime.Transient, key) where T : class;

