// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Tests.CodeOfChaos.Extensions.EntityFrameworkCore.UnitOfWork.Assets;

namespace Tests.CodeOfChaos.Extensions.EntityFrameworkCore.UnitOfWork;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ReadonlyUnitOfWorkTests {
    private Mock<MockDbContext> _dbContext = null!;
    private Mock<IDbContextFactory<MockDbContext>> _dbContextFactory = null!;
    private AsyncServiceScope _serviceScope;
    private Mock<IServiceProvider> _serviceProvider = null!;

    private ReadonlyUnitOfWork<MockDbContext> _readonlyUnitOfWork = null!;

    [Before(Test)]
    public void Setup() {
        _dbContextFactory = new Mock<IDbContextFactory<MockDbContext>>();
        _serviceProvider = new Mock<IServiceProvider>();

        // Create a real AsyncServiceScope from a ServiceCollection
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSingleton(_serviceProvider.Object);
        ServiceProvider provider = services.BuildServiceProvider();
        _serviceScope = provider.CreateAsyncScope();

        // Mock DbContextOptions with InMemory provider
        DbContextOptions<MockDbContext> options = new DbContextOptionsBuilder<MockDbContext>()
            .UseInMemoryDatabase("ReadonlyTestDatabase")
            .Options;

        _dbContext = new Mock<MockDbContext>(options) { CallBase = true };

        // Mock SetAsReadonly behavior
        _dbContextFactory
            .Setup(factory => factory.CreateDbContextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_dbContext.Object);

        var logger = new Mock<ILogger<ReadonlyUnitOfWork<MockDbContext>>>();
        _readonlyUnitOfWork = new ReadonlyUnitOfWork<MockDbContext>(_dbContextFactory.Object, _serviceScope, logger.Object);
    }

    [After(Test)]
    public async Task Cleanup() {
        await _readonlyUnitOfWork.DisposeAsync();
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Tests
    // -----------------------------------------------------------------------------------------------------------------

    [Test]
    public async Task LazyDb_ShouldCallSetAsReadonly() {
        // Arrange
        var realDbContext = new MockDbContext(new DbContextOptionsBuilder<MockDbContext>()
            .UseInMemoryDatabase("ReadonlyTestDatabase")
            .Options);

        _dbContextFactory
            .Setup(factory => factory.CreateDbContextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(realDbContext);

        // Act
        var dbContext = await _readonlyUnitOfWork.GetDbContextAsync<MockDbContext>();

        // Assert
        _dbContextFactory.Verify(factory => factory.CreateDbContextAsync(It.IsAny<CancellationToken>()), Times.Once);
        await Assert.That(dbContext.IsReadonly).IsTrue();
    }


    [Test]
    public async Task SaveChangesAsync_ShouldThrowNotSupportedException() {
        // Act & Assert
        await Assert.ThrowsAsync<NotSupportedException>(async () => await _readonlyUnitOfWork.SaveChangesAsync());
    }

    [Test]
    public async Task TryCreateTransactionAsync_ShouldThrowNotSupportedException() {
        // Act & Assert
        await Assert.ThrowsAsync<NotSupportedException>(async () => await _readonlyUnitOfWork.TryCreateTransactionAsync());
    }

    [Test]
    public async Task TryCommitTransactionAsync_ShouldThrowNotSupportedException() {
        // Act & Assert
        await Assert.ThrowsAsync<NotSupportedException>(async () => await _readonlyUnitOfWork.TryCommitTransactionAsync());
    }

    [Test]
    public async Task TryRollbackTransactionAsync_ShouldThrowNotSupportedException() {
        // Act & Assert
        await Assert.ThrowsAsync<NotSupportedException>(async () => await _readonlyUnitOfWork.TryRollbackTransactionAsync());
    }

    [Test]
    public async Task TryCreateSavepointAsync_ShouldThrowNotSupportedException() {
        // Act & Assert
        await Assert.ThrowsAsync<NotSupportedException>(async () => await _readonlyUnitOfWork.TryCreateSavepointAsync(Guid.NewGuid()));
    }

    [Test]
    public async Task TryRollbackToSavepointAsync_ShouldThrowNotSupportedException() {
        // Act & Assert
        await Assert.ThrowsAsync<NotSupportedException>(async () => await _readonlyUnitOfWork.TryRollbackToSavepointAsync(Guid.NewGuid()));
    }

    [Test]
    public async Task GetRepository_ShouldRetrieveRepositoryFromServiceProvider() {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddDbContextFactory<DefaultDbContext>();
        services.AddTransient<DefaultRepository, DefaultRepository>();
        ServiceProvider provider = services.BuildServiceProvider();
        var factory = new ReadonlyUnitOfWorkFactory<DefaultDbContext>(
            provider.GetRequiredService<IDbContextFactory<DefaultDbContext>>(),
            provider,
            provider.GetRequiredService<ILoggerFactory>()
        );
        IReadonlyUnitOfWork<DefaultDbContext> readonlyUnitOfWork = factory.Create();

        // Act
        var repository = await readonlyUnitOfWork.GetRepositoryAsync<DefaultRepository>();

        // Assert
        await Assert.That(repository).IsNotNull();
    }
}