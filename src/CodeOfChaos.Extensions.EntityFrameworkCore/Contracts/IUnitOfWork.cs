// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Microsoft.EntityFrameworkCore;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IUnitOfWork<TDbContext> : IAsyncDisposable where TDbContext : DbContext {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // ----------------------------------------------------------------------------------------------------------------
    void SaveChanges();
    ValueTask SaveChangesAsync(CancellationToken ct = default);
    
    bool TryCommitTransaction();
    ValueTask<bool> TryCommitTransactionAsync(CancellationToken ct = default);
    
    bool TryCreateTransaction();
    ValueTask<bool> TryCreateTransactionAsync(CancellationToken ct = default);
    
    bool TryRollbackTransaction();
    ValueTask<bool> TryRollbackTransactionAsync(CancellationToken ct = default);
    
    bool TryCreateSavepoint(Guid id);
    ValueTask<bool> TryCreateSavepointAsync(Guid id, CancellationToken ct = default);
    
    bool TryRollbackToSavepoint(Guid id);
    ValueTask<bool> TryRollbackToSavepointAsync(Guid id, CancellationToken ct = default);
    
    TDbContext GetDbContext();
    ValueTask<TDbContext> GetDbContextAsync(CancellationToken ct = default);

    TRepo GetRepository<TRepo>() where TRepo : class, IUnitOfWorkRepository;
    ValueTask<TRepo> GetRepositoryAsync<TRepo>(CancellationToken ct = default) where TRepo : class, IUnitOfWorkRepository;
}
