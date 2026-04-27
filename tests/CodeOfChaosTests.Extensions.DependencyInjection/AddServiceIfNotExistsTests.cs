// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;

namespace CodeOfChaosTests.Extensions.DependencyInjection;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class AddServiceIfNotExistsTests {
    [Test]
    public async Task AddServiceIfNotExists_WhenServiceDoesNotExist_AddsWithDefaultScoped() {
        // Arrange
        var services = new ServiceCollection();
        
        // Act
        services.AddServiceIfNotExists<ITestService, TestService>(ServiceLifetime.Scoped);
        ServiceDescriptor? descriptor = services.FirstOrDefault(x => x.ServiceType == typeof(ITestService));
        
        // Assert
        await Assert.That(descriptor).IsNotNull();
        await Assert.That(descriptor!.ImplementationType).IsEqualTo(typeof(TestService));
        await Assert.That(descriptor.Lifetime).IsEqualTo(ServiceLifetime.Scoped);
    }

    [Test]
    [Arguments(ServiceLifetime.Singleton)]
    [Arguments(ServiceLifetime.Scoped)]
    [Arguments(ServiceLifetime.Transient)]
    public async Task AddServiceIfNotExists_WithSpecificLifetime_AddsServiceCorrectly(ServiceLifetime lifetime) {
        // Arrange
        var services = new ServiceCollection();
        
        // Act
        services.AddServiceIfNotExists<ITestService, TestService>(lifetime);
        ServiceDescriptor? descriptor = services.FirstOrDefault(x => x.ServiceType == typeof(ITestService));
        
        // Assert
        await Assert.That(descriptor).IsNotNull();
        await Assert.That(descriptor!.Lifetime).IsEqualTo(lifetime);
    }

    [Test]
    public async Task AddSingletonIfNotExists_AddsServiceWithSingletonLifetime() {
        // Arrange
        var services = new ServiceCollection();
        
        // Act
        services.AddSingletonIfNotExists<ITestService, TestService>();
        ServiceDescriptor? descriptor = services.FirstOrDefault(x => x.ServiceType == typeof(ITestService));
        
        // Assert
        await Assert.That(descriptor).IsNotNull();
        await Assert.That(descriptor!.Lifetime).IsEqualTo(ServiceLifetime.Singleton);
    }

    [Test]
    public async Task AddScopedIfNotExists_AddsServiceWithScopedLifetime() {
        // Arrange
        var services = new ServiceCollection();
        
        // Act
        services.AddScopedIfNotExists<ITestService, TestService>();
        ServiceDescriptor? descriptor = services.FirstOrDefault(x => x.ServiceType == typeof(ITestService));
        
        // Assert
        await Assert.That(descriptor).IsNotNull();
        await Assert.That(descriptor!.Lifetime).IsEqualTo(ServiceLifetime.Scoped);
    }

    [Test]
    public async Task AddTransientIfNotExists_AddsServiceWithTransientLifetime() {
        // Arrange
        var services = new ServiceCollection();
        
        // Act
        services.AddTransientIfNotExists<ITestService, TestService>();
        ServiceDescriptor? descriptor = services.FirstOrDefault(x => x.ServiceType == typeof(ITestService));
        
        // Assert
        await Assert.That(descriptor).IsNotNull();
        await Assert.That(descriptor!.Lifetime).IsEqualTo(ServiceLifetime.Transient);
    }

    [Test]
    public async Task AddKeyedServiceIfNotExists_WhenServiceDoesNotExist_AddsWithDefaultScoped() {
        // Arrange
        var services = new ServiceCollection();
        const string key = "test-key";
        
        // Act
        services.AddKeyedServiceIfNotExists<ITestService, TestService>(key, ServiceLifetime.Scoped);
        ServiceDescriptor? descriptor = services.FirstOrDefault(x => x.ServiceType == typeof(ITestService) && ReferenceEquals(x.ServiceKey, key));
        
        // Assert
        await Assert.That(descriptor).IsNotNull();
        await Assert.That(descriptor!.KeyedImplementationType).IsEqualTo(typeof(TestService));
        await Assert.That(descriptor.Lifetime).IsEqualTo(ServiceLifetime.Scoped);
    }

    [Test]
    [Arguments(ServiceLifetime.Singleton)]
    [Arguments(ServiceLifetime.Scoped)]
    [Arguments(ServiceLifetime.Transient)]
    public async Task AddKeyedServiceIfNotExists_WithSpecificLifetime_AddsServiceCorrectly(ServiceLifetime lifetime) {
        // Arrange
        var services = new ServiceCollection();
        const string key = "test-key";
        
        // Act
        services.AddKeyedServiceIfNotExists<ITestService, TestService>(key, lifetime);
        ServiceDescriptor? descriptor = services.FirstOrDefault(x => x.ServiceType == typeof(ITestService) && ReferenceEquals(x.ServiceKey, key));
        
        // Assert
        await Assert.That(descriptor).IsNotNull();
        await Assert.That(descriptor!.Lifetime).IsEqualTo(lifetime);
    }

    [Test]
    public async Task AddKeyedSingletonIfNotExists_AddsServiceWithSingletonLifetime() {
        // Arrange
        var services = new ServiceCollection();
        string key = "test-key";
        
        // Act
        services.AddKeyedSingletonIfNotExists<ITestService, TestService>(key);
        ServiceDescriptor? descriptor = services.FirstOrDefault(x => x.ServiceType == typeof(ITestService) && ReferenceEquals(x.ServiceKey, key));
        
        // Assert
        await Assert.That(descriptor).IsNotNull();
        await Assert.That(descriptor!.Lifetime).IsEqualTo(ServiceLifetime.Singleton);
    }

    [Test]
    public async Task AddKeyedScopedIfNotExists_AddsServiceWithScopedLifetime() {
        // Arrange
        var services = new ServiceCollection();
        const string key = "test-key";
        
        // Act
        services.AddKeyedScopedIfNotExists<ITestService, TestService>(key);
        ServiceDescriptor? descriptor = services.FirstOrDefault(x => x.ServiceType == typeof(ITestService) && ReferenceEquals(x.ServiceKey, key));
        
        // Assert
        await Assert.That(descriptor).IsNotNull();
        await Assert.That(descriptor!.Lifetime).IsEqualTo(ServiceLifetime.Scoped);
    }

    [Test]
    public async Task AddKeyedTransientIfNotExists_AddsServiceWithTransientLifetime() {
        // Arrange
        var services = new ServiceCollection();
        const string key = "test-key";
        
        // Act
        services.AddKeyedTransientIfNotExists<ITestService, TestService>(key);
        ServiceDescriptor? descriptor = services.FirstOrDefault(x => x.ServiceType == typeof(ITestService) && ReferenceEquals(x.ServiceKey, key));
        
        // Assert
        await Assert.That(descriptor).IsNotNull();
        await Assert.That(descriptor!.Lifetime).IsEqualTo(ServiceLifetime.Transient);
    }

    [Test]
    public async Task AddKeyedServiceIfNotExists_WhenServiceExists_DoesNotAddDuplicate() {
        // Arrange
        var services = new ServiceCollection();
        const string key = "test-key";
        services.AddKeyedScoped<ITestService, TestService>(key);
        int initialCount = services.Count;
        
        // Act
        services.AddKeyedServiceIfNotExists<ITestService, TestService>(key, ServiceLifetime.Singleton);
        
        // Assert
        await Assert.That(services.Count).IsEqualTo(initialCount);
    }

    // Test interfaces and classes
    private interface ITestService { }
    private class TestService : ITestService { }
}