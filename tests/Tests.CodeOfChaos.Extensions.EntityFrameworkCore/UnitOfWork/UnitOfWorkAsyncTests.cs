// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Tests.CodeOfChaos.Extensions.EntityFrameworkCore.UnitOfWork.Assets;

namespace Tests.CodeOfChaos.Extensions.EntityFrameworkCore.UnitOfWork;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UnitOfWorkAsyncTests {
    private Mock<MockDbContext> _dbContext = null!;
    private Mock<IDbContextFactory<MockDbContext>> _dbContextFactory = null!;
    private Mock<IDbContextTransaction> _dbTransaction = null!;
    private Mock<IServiceProvider> _serviceProvider = null!;
    private AsyncServiceScope _serviceScope;

    private UnitOfWork<MockDbContext> _unitOfWork = null!;

    [Before(Test)]
    public void Setup() {
        _dbContextFactory = new Mock<IDbContextFactory<MockDbContext>>();
        _serviceProvider = new Mock<IServiceProvider>();

        // Create a real AsyncServiceScope from a ServiceCollection
        var services = new ServiceCollection();
        services.AddSingleton(_serviceProvider.Object);
        ServiceProvider provider = services.BuildServiceProvider();
        _serviceScope = provider.CreateAsyncScope();

        // Configure DbContextOptions with the InMemory provider
        DbContextOptions<MockDbContext> options = new DbContextOptionsBuilder<MockDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        // Create a real MockDbContext (no Moq here since Moq/mocking DbContext often encounters internal EF issues)
        _dbContext = new Mock<MockDbContext>(options) { CallBase = true };

        _dbTransaction = new Mock<IDbContextTransaction>();

        // Mock the DatabaseFacade for the DbContext and its transaction behavior
        var mockDatabaseFacade = new Mock<DatabaseFacade>(_dbContext.Object);

        mockDatabaseFacade
            .Setup(db => db.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_dbTransaction.Object);

        mockDatabaseFacade
            .Setup(db => db.CurrentTransaction)
            .Returns(() => _dbTransaction.Object);

        _dbTransaction
            .Setup(t => t.SupportsSavepoints)
            .Returns(true);

        _dbContext
            .Setup(db => db.Database)
            .Returns(mockDatabaseFacade.Object);

        _dbContextFactory
            .Setup(d => d.CreateDbContextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_dbContext.Object);

        _unitOfWork = new UnitOfWork<MockDbContext>(_dbContextFactory.Object, _serviceScope);
    }

    [After(Test)]
    public async Task CleanupAsync() {
        if (_unitOfWork.IsDisposed) return;
        await _unitOfWork.DisposeAsync();
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Tests
    // -----------------------------------------------------------------------------------------------------------------

    [Test]
    public async Task SaveChangesAsync_ShouldSaveChangesInDbContext() {
        // Act
        await _unitOfWork.SaveChangesAsync();

        // Assert
        _dbContext.Verify(expression: db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task TryCreateTransactionAsync_ShouldBeginNewTransaction() {
        // Arrange
        _dbContext.Setup(db => db.Database.CurrentTransaction).Returns(() => null);
        _dbContext.Setup(db => db.Database.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_dbTransaction.Object);

        // Act
        bool result = await _unitOfWork.TryCreateTransactionAsync();

        // Assert
        await Assert.That(result).IsTrue();
        _dbContext.Verify(expression: db => db.Database.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task TryCommitTransactionAsync_ShouldCommitTransaction() {
        // Arrange
        _dbContext.Setup(db => db.Database.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_dbTransaction.Object);

        await _unitOfWork.TryCreateTransactionAsync();

        // Act
        bool result = await _unitOfWork.TryCommitTransactionAsync();

        // Assert
        await Assert.That(result).IsTrue();
        _dbTransaction.Verify(expression: transaction => transaction.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        _dbTransaction.Verify(expression: transaction => transaction.DisposeAsync(), Times.Once);
    }

    [Test]
    public async Task TryRollbackTransactionAsync_ShouldRollbackTransaction() {
        // Arrange
        _dbContext.Setup(db => db.Database.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_dbTransaction.Object);

        await _unitOfWork.TryCreateTransactionAsync();

        // Act
        bool result = await _unitOfWork.TryRollbackTransactionAsync();

        // Assert
        await Assert.That(result).IsTrue();
        _dbTransaction.Verify(expression: transaction => transaction.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
        _dbTransaction.Verify(expression: transaction => transaction.DisposeAsync(), Times.Once);
    }

    [Test]
    public async Task TryCreateSavepointAsync_ShouldCreateSavepoint() {
        // Arrange
        var savepointId = Guid.NewGuid();
        _dbContext.Setup(db => db.Database.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_dbTransaction.Object);

        _dbTransaction.Setup(t => t.SupportsSavepoints).Returns(true);

        await _unitOfWork.TryCreateTransactionAsync();

        // Act
        bool result = await _unitOfWork.TryCreateSavepointAsync(savepointId);

        // Assert
        await Assert.That(result).IsTrue();
        _dbTransaction.Verify(expression: transaction => transaction.CreateSavepointAsync(savepointId.ToString("N"), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task TryRollbackToSavepointAsync_ShouldRollbackToSpecificSavepoint() {
        // Arrange
        var savepointId = Guid.NewGuid();
        _dbContext.Setup(db => db.Database.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_dbTransaction.Object);

        _dbTransaction.Setup(t => t.SupportsSavepoints).Returns(true);

        await _unitOfWork.TryCreateTransactionAsync();

        // Act
        bool result = await _unitOfWork.TryRollbackToSavepointAsync(savepointId);

        // Assert
        await Assert.That(result).IsTrue();
        _dbTransaction.Verify(expression: transaction => transaction.RollbackToSavepointAsync(savepointId.ToString("N"), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task GetDbContextAsync_ShouldReturnDbContext() {
        // Act
        var dbContext = await _unitOfWork.GetDbContextAsync<MockDbContext>();

        // Assert
        _dbContextFactory.Verify(expression: factory => factory.CreateDbContextAsync(It.IsAny<CancellationToken>()), Times.Once);
        await Assert.That(dbContext).IsNotNull();
        await Assert.That(_dbContext.Object).IsEqualTo(dbContext);
    }

    [Test]
    public async Task GetDbContextAsync_ShouldThrowForUnsupportedDbContextType() {
        // Act & Assert
        await Assert.ThrowsAsync<NotSupportedException>(async () => await _unitOfWork.GetDbContextAsync<OtherDbContext>());
    }

    [Test]
    public async Task GetRepositoryAsync_ShouldRetrieveRepositoryFromServiceProvider() {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddDbContextFactory<DefaultDbContext>();
        services.AddUnitOfWork<DefaultDbContext>();
        services.AddTransient<DefaultRepository>();
        ServiceProvider provider = services.BuildServiceProvider();
        IUnitOfWork<DefaultDbContext> unitOfWork = provider.GetRequiredService<IUnitOfWorkFactory<DefaultDbContext>>().Create();

        // Act
        var repository = await unitOfWork.GetRepositoryAsync<DefaultRepository>();

        // Assert
        await Assert.That(repository).IsNotNull();
    }

    [Test]
    public async Task DisposeAsync_ShouldDisposeUnitOfWorkProperly() {
        // Arrange
        _dbContext.Setup(db => db.Database.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_dbTransaction.Object);

        await _unitOfWork.TryCreateTransactionAsync();

        // Act
        await _unitOfWork.DisposeAsync();

        // Assert
        _dbTransaction.Verify(expression: transaction => transaction.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
        _dbTransaction.Verify(expression: transaction => transaction.DisposeAsync(), Times.Once);
    }
}
