// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.CodeOfChaos.Extensions.DependencyInjection.Attributes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(InjectableServiceAttribute<>))]
public class InjectableServiceAttributeTests {
    [Test]
    public async Task InjectableServiceAttribute_PropertiesAreSetCorrectly() {
        // Arrange
        var attribute = new InjectableServiceAttribute<IMyService>(ServiceLifetime.Scoped);

        // Act
        ServiceLifetime lifetime = attribute.Lifetime;
        Type serviceType = attribute.ServiceType;

        // Assert
        await Assert.That(lifetime).IsEqualTo(ServiceLifetime.Scoped);
        await Assert.That(serviceType).IsEqualTo(typeof(IMyService));
    }

    [Test]
    public async Task InjectableServiceAttribute_CanBeAppliedToClass() {
        // Act
        object[] attributes = typeof(SampleServiceWithAttribute).GetCustomAttributes(typeof(InjectableServiceAttribute<IMyService>), false);
    
        var attribute = attributes.FirstOrDefault() as InjectableServiceAttribute<IMyService>;
        
        // Assert
        await Assert.That(attributes).IsNotEmpty().And.HasSingleItem();
        await Assert.That(attribute).IsNotNull();
        await Assert.That(attribute?.Lifetime).IsEqualTo(ServiceLifetime.Singleton);
    }

    // Dummy class to test attribute application
    [InjectableService<IMyService>(ServiceLifetime.Singleton)]
    public class SampleServiceWithAttribute : IMyService;

    public interface IMyService;
}
