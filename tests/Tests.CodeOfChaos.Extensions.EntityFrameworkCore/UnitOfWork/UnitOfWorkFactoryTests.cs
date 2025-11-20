// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Tests.CodeOfChaos.Extensions.EntityFrameworkCore.UnitOfWork.Assets;

namespace Tests.CodeOfChaos.Extensions.EntityFrameworkCore.UnitOfWork;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UnitOfWorkFactoryTests {
    private Mock<IDbContextFactory<MockDbContext>> _dbContextFactory = null!;
    private UnitOfWorkFactory<MockDbContext> _factory = null!;
    private Mock<ILogger<UnitOfWorkFactory<MockDbContext>>> _logger = null!;
    private Mock<IServiceScope> _scope = null!;
    private Mock<IServiceScopeFactory> _scopeFactory = null!;
    private Mock<IServiceProvider> _serviceProvider = null!;
    private Mock<IServiceScope> _serviceScope = null!;

    // -----------------------------------------------------------------------------------------------------------------
    // Setup
    // -----------------------------------------------------------------------------------------------------------------
    [Before(Test)]
    public void Setup() {
        _dbContextFactory = new Mock<IDbContextFactory<MockDbContext>>();
        _serviceProvider = new Mock<IServiceProvider>();
        _serviceScope = new Mock<IServiceScope>();
        _logger = new Mock<ILogger<UnitOfWorkFactory<MockDbContext>>>();
        _scopeFactory = new Mock<IServiceScopeFactory>();
        _scope = new Mock<IServiceScope>();

        // Mock DbContext behavior
        Mock<MockDbContext> mockDbContext = new Mock<MockDbContext>();

        // Mock the DatabaseFacade explicitly
        Mock<DatabaseFacade> mockDatabaseFacade = new Mock<DatabaseFacade>(mockDbContext.Object);
        mockDatabaseFacade
            .Setup(db => db.CurrentTransaction)
            .Returns(() => null);

        mockDbContext
            .Setup(db => db.Database)
            .Returns(mockDatabaseFacade.Object);

        _dbContextFactory
            .Setup(factory => factory.CreateDbContextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockDbContext.Object);

        // Mock IServiceScopeFactory and IServiceScope
        _serviceProvider
            .Setup(sp => sp.GetService(typeof(IServiceScopeFactory)))
            .Returns(_scopeFactory.Object);

        _scopeFactory
            .Setup(sf => sf.CreateScope())
            .Returns(_scope.Object);

        _serviceScope
            .Setup(s => s.ServiceProvider)
            .Returns(_serviceProvider.Object);

        // Create test factory
        var loggerFactoryMock = new Mock<ILoggerFactory>();
        _factory = new UnitOfWorkFactory<MockDbContext>(
            _dbContextFactory.Object,
            _serviceProvider.Object,
            _logger.Object,
            loggerFactoryMock.Object
        );
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Tests
    // -----------------------------------------------------------------------------------------------------------------
    [Test]
    public async Task Create_ShouldReturnUnitOfWork() {
        // Act
        IUnitOfWork<MockDbContext> unitOfWork = _factory.Create();

        // Assert
        await Assert.That(unitOfWork).IsNotNull();
        await Assert.That(unitOfWork).IsTypeOf<UnitOfWork<MockDbContext>>();
        _scopeFactory.Verify(expression: sf => sf.CreateScope(), Times.Once);
    }

    [Test]
    public async Task CreateWithTransactionAsync_ShouldCreateUnitOfWorkWithTransaction() {
        // Arrange: Mock DbContext
        _serviceProvider
            .Setup(sp => sp.GetService(typeof(IServiceScopeFactory)))
            .Returns(_scopeFactory.Object);

        _scopeFactory
            .Setup(sf => sf.CreateScope())
            .Returns(_scope.Object);

        _scope
            .Setup(s => s.ServiceProvider)
            .Returns(_serviceProvider.Object);

        // Act
        IUnitOfWork<MockDbContext> unitOfWork = await _factory.CreateWithTransactionAsync();

        // Assert: Validate that UnitOfWork is created and a transaction is attempted
        await Assert.That(unitOfWork).IsNotNull();
        await Assert.That(unitOfWork).IsTypeOf<UnitOfWork<MockDbContext>>();
        _scopeFactory.Verify(expression: sf => sf.CreateScope(), Times.Once);
    }
}
