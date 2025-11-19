// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ReadonlyUnitOfWork<TDbContext>(
    IDbContextFactory<TDbContext> dbContextFactory,
    AsyncServiceScope serviceScope
) : UnitOfWork<TDbContext>(dbContextFactory, serviceScope), IReadonlyUnitOfWork<TDbContext>
    where TDbContext : DbContext, IReadonlyCapableDbContext {
    
    public async override ValueTask<TDbContext> GetDbContextAsync(CancellationToken ct) {
        TDbContext dbContext = await base.GetDbContextAsync(ct);
        dbContext.SetAsReadonly();
        return dbContext;
    }

    public override TDbContext GetDbContext() {
        TDbContext dbContext = base.GetDbContext();
        dbContext.SetAsReadonly();
        return dbContext;
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void SaveChanges() 
        => throw new NotSupportedException("ReadonlyUnitOfWork cannot save changes.");

    public override ValueTask SaveChangesAsync(CancellationToken ct = default) 
        => throw new NotSupportedException("ReadonlyUnitOfWork cannot save changes.");

    public override bool TryCommitTransaction() 
        => throw new NotSupportedException("ReadonlyUnitOfWork cannot commit transactions.");
    
    public override ValueTask<bool> TryCommitTransactionAsync(CancellationToken ct = default) 
        => throw new NotSupportedException("ReadonlyUnitOfWork cannot commit transactions.");
    
    public override bool TryCreateTransaction() 
        => throw new NotSupportedException("ReadonlyUnitOfWork cannot create transactions.");
    
    public override ValueTask<bool> TryCreateTransactionAsync(CancellationToken ct = default) 
        => throw new NotSupportedException("ReadonlyUnitOfWork cannot create transactions.");
    
    public override bool TryRollbackTransaction() 
        => throw new NotSupportedException("ReadonlyUnitOfWork cannot rollback transactions.");

    public override ValueTask<bool> TryRollbackTransactionAsync(CancellationToken ct = default) 
        => throw new NotSupportedException("ReadonlyUnitOfWork cannot rollback transactions.");
    
    public override bool TryRollbackToSavepoint(Guid id) 
        => throw new NotSupportedException("ReadonlyUnitOfWork cannot rollback to savepoints.");

    public override ValueTask<bool> TryRollbackToSavepointAsync(Guid id, CancellationToken ct = default) 
        => throw new NotSupportedException("ReadonlyUnitOfWork cannot rollback to savepoints.");
    
    public override bool TryCreateSavepoint(Guid id) 
        => throw new NotSupportedException("ReadonlyUnitOfWork cannot create savepoints.");

    public override ValueTask<bool> TryCreateSavepointAsync(Guid id, CancellationToken ct = default) 
        => throw new NotSupportedException("ReadonlyUnitOfWork cannot create savepoints.");
}
