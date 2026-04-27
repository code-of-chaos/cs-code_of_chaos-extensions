// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using CodeOfChaosTests.Extensions.EntityFrameworkCore.UnitOfWork.Assets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace CodeOfChaosTests.Extensions.EntityFrameworkCore.UnitOfWork;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[SuppressMessage("ReSharper", "MethodHasAsyncOverload")]
public class UnitOfWorkSyncTests {
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
            .Setup(db => db.BeginTransaction())
            .Returns(_dbTransaction.Object);

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
            .Setup(d => d.CreateDbContext())
            .Returns(_dbContext.Object);

        var logger = new Mock<ILogger<UnitOfWork<MockDbContext>>>();
        _unitOfWork = new UnitOfWork<MockDbContext>(_dbContextFactory.Object, _serviceScope, logger.Object);
    }

    [After(Test)]
    public async Task CleanupAsync() {
        await _unitOfWork.DisposeAsync();
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Tests
    // -----------------------------------------------------------------------------------------------------------------

    [Test]
    public void SaveChanges_ShouldSaveChangesInDbContext() {
        // Act
        _unitOfWork.SaveChanges();

        // Assert
        _dbContext.Verify(expression: db => db.SaveChanges(), Times.Once);
    }

    [Test]
    public async Task TryCreateTransaction_ShouldBeginNewTransaction() {
        // Arrange
        _dbContext.Setup(db => db.Database.CurrentTransaction).Returns(() => null);
        _dbContext.Setup(db => db.Database.BeginTransaction())
            .Returns(_dbTransaction.Object);

        // Act
        bool result = _unitOfWork.TryCreateTransaction();

        // Assert
        await Assert.That(result).IsTrue();
        _dbContext.Verify(expression: db => db.Database.BeginTransaction(), Times.Once);
    }

    [Test]
    public async Task TryCommitTransaction_ShouldCommitTransaction() {
        // Arrange
        _dbContext.Setup(db => db.Database.BeginTransaction())
            .Returns(_dbTransaction.Object);

        _unitOfWork.TryCreateTransaction();

        // Act
        bool result = _unitOfWork.TryCommitTransaction();

        // Assert
        await Assert.That(result).IsTrue();
        _dbTransaction.Verify(expression: transaction => transaction.Commit(), Times.Once);
        _dbTransaction.Verify(expression: transaction => transaction.Dispose(), Times.Once);
    }

    [Test]
    public async Task TryRollbackTransaction_ShouldRollbackTransaction() {
        // Arrange
        _dbContext.Setup(db => db.Database.BeginTransaction())
            .Returns(_dbTransaction.Object);

        _unitOfWork.TryCreateTransaction();

        // Act
        bool result = _unitOfWork.TryRollbackTransaction();

        // Assert
        await Assert.That(result).IsTrue();
        _dbTransaction.Verify(expression: transaction => transaction.Rollback(), Times.Once);
        _dbTransaction.Verify(expression: transaction => transaction.Dispose(), Times.Once);
    }

    [Test]
    public async Task TryCreateSavepoint_ShouldCreateSavepoint() {
        // Arrange
        var savepointId = Guid.NewGuid();
        _dbContext.Setup(db => db.Database.BeginTransaction())
            .Returns(_dbTransaction.Object);

        _dbTransaction.Setup(t => t.SupportsSavepoints).Returns(true);

        _unitOfWork.TryCreateTransaction();

        // Act
        bool result = _unitOfWork.TryCreateSavepoint(savepointId);

        // Assert
        await Assert.That(result).IsTrue();
        _dbTransaction.Verify(expression: transaction => transaction.CreateSavepoint(savepointId.ToString("N")), Times.Once);
    }

    [Test]
    public async Task TryRollbackToSavepoint_ShouldRollbackToSpecificSavepoint() {
        // Arrange
        var savepointId = Guid.NewGuid();
        _dbContext.Setup(db => db.Database.BeginTransaction())
            .Returns(_dbTransaction.Object);

        _dbTransaction.Setup(t => t.SupportsSavepoints).Returns(true);

        _unitOfWork.TryCreateTransaction();

        // Act
        bool result = _unitOfWork.TryRollbackToSavepoint(savepointId);

        // Assert
        await Assert.That(result).IsTrue();
        _dbTransaction.Verify(expression: transaction => transaction.RollbackToSavepoint(savepointId.ToString("N")), Times.Once);
    }

    [Test]
    public async Task GetDbContext_ShouldReturnDbContext() {
        // Act
        var dbContext = _unitOfWork.GetDbContext<MockDbContext>();

        // Assert
        _dbContextFactory.Verify(expression: factory => factory.CreateDbContext(), Times.Once);
        await Assert.That(dbContext).IsNotNull();
        await Assert.That(_dbContext.Object).IsEqualTo(dbContext);
    }

    [Test]
    public void GetDbContext_ShouldThrowForUnsupportedDbContextType() {
        // Act & Assert
        Assert.Throws<NotSupportedException>(() => _unitOfWork.GetDbContext<OtherDbContext>());
    }

    [Test]
    public async Task GetRepository_ShouldRetrieveRepositoryFromServiceProvider() {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddDbContextFactory<DefaultDbContext>();
        services.AddUnitOfWork<DefaultDbContext>();
        services.AddTransient<DefaultRepository>();
        ServiceProvider provider = services.BuildServiceProvider();
        IUnitOfWork<DefaultDbContext> unitOfWork = provider.GetRequiredService<IUnitOfWorkFactory<DefaultDbContext>>().Create();

        // Act
        var repository = unitOfWork.GetRepository<DefaultRepository>();

        // Assert
        await Assert.That(repository).IsNotNull();
    }
}
