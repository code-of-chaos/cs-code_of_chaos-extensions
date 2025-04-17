// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;

namespace Tests.CodeOfChaos.Extensions.DependencyInjection;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class AddServiceIfNotExistsTests {
    [Test]
    public async Task AddServiceIfNotExists_WhenServiceDoesNotExist_AddsWithDefaultScoped() {
        // Arrange
        var services = new ServiceCollection();
        
        // Act
        services.AddServiceIfNotExists<ITestService, TestService>();
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
    public async Task AddServiceIfNotExists_WhenServiceExists_DoesNotAddDuplicate() {
        // Arrange
        var services = new ServiceCollection();
        services.AddScoped<ITestService, TestService>();
        int initialCount = services.Count;
        
        // Act
        services.AddServiceIfNotExists<ITestService, TestService>();
        
        // Assert
        await Assert.That(services.Count).IsEqualTo(initialCount);
    }

    [Test]
    public async Task AddServiceIfNotExists_WithInvalidLifetime_ThrowsException() {
        // Arrange
        var services = new ServiceCollection();
        
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => {
            services.AddServiceIfNotExists<ITestService, TestService>((ServiceLifetime)999);
            return Task.CompletedTask;
        });
    }

    // Test interfaces and classes
    private interface ITestService { }
    private class TestService : ITestService { }
}
