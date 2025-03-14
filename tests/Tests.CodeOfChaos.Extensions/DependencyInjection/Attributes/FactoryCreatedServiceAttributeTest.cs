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
[TestSubject(typeof(FactoryCreatedServiceAttribute<,>))]
public class FactoryCreatedServiceAttributeTest {
    [Test]
    public async Task FactoryCreatedServiceAttribute_PropertiesAreSetCorrectly() {
        // Arrange
        var attribute = new FactoryCreatedServiceAttribute<MyServiceFactory, IMyService>(ServiceLifetime.Scoped);

        // Act
        ServiceLifetime lifetime = attribute.Lifetime;
        Type factoryType = attribute.FactoryType;
        Type serviceType = attribute.ServiceType;

        // Assert
        await Assert.That(lifetime).IsEqualTo(ServiceLifetime.Scoped);
        await Assert.That(factoryType).IsEqualTo(typeof(MyServiceFactory));
        await Assert.That(serviceType).IsEqualTo(typeof(IMyService));
    }

    [Test]
    public async Task FactoryCreatedServiceAttribute_CanBeAppliedToClass() {
        // Act
        object[] attributes = typeof(SampleServiceWithAttribute).GetCustomAttributes(typeof(FactoryCreatedServiceAttribute<MyServiceFactory, IMyService>), false);
        var attribute = attributes.FirstOrDefault() as FactoryCreatedServiceAttribute<MyServiceFactory, IMyService>;
        
        // Assert
        await Assert.That(attributes).IsNotEmpty().And.HasSingleItem();
        await Assert.That(attribute).IsNotNull();
        await Assert.That(attribute?.Lifetime).IsEqualTo(ServiceLifetime.Singleton);
    }

    // Dummy class to test attribute application
    [FactoryCreatedService<MyServiceFactory, IMyService>(ServiceLifetime.Singleton)]
    private class SampleServiceWithAttribute;

    public interface IMyService;

    public class MyService : IMyService;

    public class MyServiceFactory : IFactoryService<IMyService> {
        public IMyService Create() => new MyService();
    }
}
