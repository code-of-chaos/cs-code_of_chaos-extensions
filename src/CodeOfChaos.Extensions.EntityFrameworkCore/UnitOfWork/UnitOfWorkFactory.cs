// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.EntityFrameworkCore;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UnitOfWorkFactory<TDbContext>(IDbContextFactory<TDbContext> dbContextFactory, IServiceProvider provider, ILogger<UnitOfWorkFactory<TDbContext>> logger, ILoggerFactory loggerFactory) : IUnitOfWorkFactory<TDbContext> where TDbContext : DbContext {
    public IUnitOfWork<TDbContext> Create() {
        // Each unit of work should have their own scope which they pull their repositories from
        //      This, if the factory is used correctly, should enforce correct usage and limit dbcontext concurrency issues.
        AsyncServiceScope scope = provider.CreateAsyncScope();

        // Because our factory doesn't create the actual dbcontext, yet we are safe, and we can just inject it downwards.
        return new UnitOfWork<TDbContext>(dbContextFactory, scope, loggerFactory.CreateLogger<UnitOfWork<TDbContext>>());
    }

    public IUnitOfWork<TDbContext> CreateWithTransaction() {
        IUnitOfWork<TDbContext> unitOfWork = Create();

        // ReSharper disable once InvertIf
        if (!unitOfWork.TryCreateTransaction()) {
            logger.LogError("Failed to create transaction for new unit of work.");
            throw new Exception("Failed to create transaction");
        }
        
        return unitOfWork;
    }

    public async ValueTask<IUnitOfWork<TDbContext>> CreateWithTransactionAsync(CancellationToken ct = default) {
        IUnitOfWork<TDbContext> unitOfWork = Create();

        // ReSharper disable once InvertIf
        if (!await unitOfWork.TryCreateTransactionAsync(ct)) {
            logger.LogError("Failed to create transaction for new unit of work.");
            throw new Exception("Failed to create transaction");
        }

        return unitOfWork;
    }
    
    public bool TryCreateWithTransaction([NotNullWhen(true)] out IUnitOfWork<TDbContext>? unitOfWork) {
        unitOfWork = Create();

        // ReSharper disable once InvertIf
        if (!unitOfWork.TryCreateTransaction()) {
            unitOfWork = null;
            return false;
        }
        return true;
    }

    public async ValueTask<IUnitOfWork<TDbContext>?> TryCreateWithTransactionAsync(CancellationToken ct = default) {
        IUnitOfWork<TDbContext> unitOfWork = Create();

        // ReSharper disable once InvertIf
        if (!await unitOfWork.TryCreateTransactionAsync(ct)) {
            logger.LogError("Failed to create transaction for new unit of work.");
            return null;
        }

        return unitOfWork;
    }
}
