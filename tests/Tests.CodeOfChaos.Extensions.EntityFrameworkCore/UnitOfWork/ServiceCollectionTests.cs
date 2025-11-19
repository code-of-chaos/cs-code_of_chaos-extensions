// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tests.CodeOfChaos.Extensions.EntityFrameworkCore.UnitOfWork.Assets;

namespace Tests.CodeOfChaos.Extensions.EntityFrameworkCore.UnitOfWork;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ServiceCollectionTests {
    private IServiceCollection _services = null!;

    [Before(Test)]
    public void Setup() {
        _services = new ServiceCollection();
        _services.AddLogging();
        _services.AddDbContextFactory<DefaultDbContext>();
        _services.AddDbContextFactory<OtherDbContext>();
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Tests
    // -----------------------------------------------------------------------------------------------------------------

    [Test]
    public async Task AddUnitOfWork_ShouldRegister_UnitOfWorkServices() {
        // Arrange
        _services.AddUnitOfWork<DefaultDbContext>();

        ServiceProvider provider = _services.BuildServiceProvider();

        // Act
        var unitOfWorkFactory = provider.GetService<IUnitOfWorkFactory<DefaultDbContext>>();
        var unitOfWork = provider.GetService<IUnitOfWork<DefaultDbContext>>();

        // Assert
        await Assert.That(unitOfWorkFactory).IsNotNull()
            .And.IsTypeOf<UnitOfWorkFactory<DefaultDbContext>>();

        await Assert.That(unitOfWork).IsNotNull()
            .And.IsTypeOf<UnitOfWork<DefaultDbContext>>();
    }

    [Test]
    public async Task AddUnitOfWork_ShouldRegister_UnitOfWorkServices_WithKeyedService() {
        // Arrange
        _services.AddUnitOfWork<DefaultDbContext>("default");
        _services.AddUnitOfWork<OtherDbContext>("other");

        ServiceProvider provider = _services.BuildServiceProvider();

        // Act
        var defaultUnitOfWorkFactory = provider.GetKeyedService<IUnitOfWorkFactory<DefaultDbContext>>("default");
        var defaultUnitOfWork = provider.GetKeyedService<IUnitOfWork<DefaultDbContext>>("default");
        var otherUnitOfWorkFactory = provider.GetKeyedService<IUnitOfWorkFactory<OtherDbContext>>("other");
        var otherUnitOfWork = provider.GetKeyedService<IUnitOfWork<OtherDbContext>>("other");

        // Assert
        await Assert.That(defaultUnitOfWorkFactory).IsNotNull()
            .And.IsTypeOf<UnitOfWorkFactory<DefaultDbContext>>();

        await Assert.That(defaultUnitOfWork).IsNotNull()
            .And.IsTypeOf<UnitOfWork<DefaultDbContext>>();

        await Assert.That(otherUnitOfWorkFactory).IsNotNull()
            .And.IsTypeOf<UnitOfWorkFactory<OtherDbContext>>();

        await Assert.That(otherUnitOfWork).IsNotNull()
            .And.IsTypeOf<UnitOfWork<OtherDbContext>>();
    }
    
    [Test]
    public async Task AddReadonlyUnitOfWork_ShouldRegister_ReadonlyUnitOfWorkServices() {
        // Arrange
        _services.AddReadonlyUnitOfWork<DefaultDbContext>();

        ServiceProvider provider = _services.BuildServiceProvider();

        // Act
        var readonlyUnitOfWorkFactory = provider.GetService<IReadonlyUnitOfWorkFactory<DefaultDbContext>>();
        var readonlyUnitOfWork = provider.GetService<IReadonlyUnitOfWork<DefaultDbContext>>();

        // Assert
        await Assert.That(readonlyUnitOfWorkFactory).IsNotNull()
            .And.IsTypeOf<ReadonlyUnitOfWorkFactory<DefaultDbContext>>();

        await Assert.That(readonlyUnitOfWork).IsNotNull()
            .And.IsTypeOf<ReadonlyUnitOfWork<DefaultDbContext>>();
    }

    [Test]
    public async Task AddReadonlyUnitOfWork_ShouldRegister_ReadonlyUnitOfWorkServices_WithKeyedService() {
        // Arrange
        _services.AddReadonlyUnitOfWork<DefaultDbContext>("default");
        _services.AddReadonlyUnitOfWork<OtherDbContext>("other");

        ServiceProvider provider = _services.BuildServiceProvider();

        // Act
        var defaultReadonlyUnitOfWorkFactory = provider.GetKeyedService<IReadonlyUnitOfWorkFactory<DefaultDbContext>>("default");
        var defaultReadonlyUnitOfWork = provider.GetKeyedService<IReadonlyUnitOfWork<DefaultDbContext>>("default");
        var otherReadonlyUnitOfWorkFactory = provider.GetKeyedService<IReadonlyUnitOfWorkFactory<OtherDbContext>>("other");
        var otherReadonlyUnitOfWork = provider.GetKeyedService<IReadonlyUnitOfWork<OtherDbContext>>("other");

        // Assert
        await Assert.That(defaultReadonlyUnitOfWorkFactory).IsNotNull()
            .And.IsTypeOf<ReadonlyUnitOfWorkFactory<DefaultDbContext>>();

        await Assert.That(defaultReadonlyUnitOfWork).IsNotNull()
            .And.IsTypeOf<ReadonlyUnitOfWork<DefaultDbContext>>();

        await Assert.That(otherReadonlyUnitOfWorkFactory).IsNotNull()
            .And.IsTypeOf<ReadonlyUnitOfWorkFactory<OtherDbContext>>();

        await Assert.That(otherReadonlyUnitOfWork).IsNotNull()
            .And.IsTypeOf<ReadonlyUnitOfWork<OtherDbContext>>();
    }

}
