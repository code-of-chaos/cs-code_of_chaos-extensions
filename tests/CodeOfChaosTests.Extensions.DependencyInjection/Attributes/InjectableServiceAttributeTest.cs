// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace CodeOfChaosTests.Extensions.DependencyInjection.Attributes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(InjectableAttribute<>))]
public class InjectableAttributeTests {
    [Test]
    public async Task InjectableAttribute_PropertiesAreSetCorrectly() {
        // Arrange
        var attribute = new InjectableAttribute<IMyService>(ServiceLifetime.Scoped);

        // Act
        ServiceLifetime lifetime = attribute.Lifetime;
        Type serviceType = attribute.ServiceType;

        // Assert
        await Assert.That(lifetime).IsEqualTo(ServiceLifetime.Scoped);
        await Assert.That(serviceType).IsEqualTo(typeof(IMyService));
    }

    [Test]
    public async Task InjectableAttribute_CanBeAppliedToClass() {
        // Act
        object[] attributes = typeof(SampleServiceWithAttribute).GetCustomAttributes(typeof(InjectableAttribute<IMyService>), false);

        var attribute = attributes.FirstOrDefault() as InjectableAttribute<IMyService>;

        // Assert
        await Assert.That(attributes).IsNotEmpty().And.HasSingleItem();
        await Assert.That(attribute).IsNotNull();
        await Assert.That(attribute?.Lifetime).IsEqualTo(ServiceLifetime.Singleton);
    }

    // Dummy class to test attribute application
    [Injectable<IMyService>(ServiceLifetime.Singleton)]
    public class SampleServiceWithAttribute : IMyService;

    public interface IMyService;
}
