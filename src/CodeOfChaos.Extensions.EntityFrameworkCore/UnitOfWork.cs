// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace Microsoft.EntityFrameworkCore;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UnitOfWork<TDbContext>(IDbContextFactory<TDbContext> dbContextFactory, AsyncServiceScope serviceScope) : IUnitOfWork<TDbContext> where TDbContext : DbContext {
    private TDbContext? _dbContext;
    private IDbContextTransaction? _transaction;
    private readonly ConcurrentDictionary<Type, IUnitOfWorkRepository> AttachedRepositories = [];
    private readonly SemaphoreSlim _initLockAsync = new(1, 1);
    private readonly SemaphoreSlim _transactionLockAsync = new(1, 1);
    private readonly Lock _transactionLock = new();
    private readonly Lazy<TDbContext> _lazyDb = new(dbContextFactory.CreateDbContext, LazyThreadSafetyMode.ExecutionAndPublication);
    
    internal bool IsDisposed { get; private set; }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public virtual async ValueTask<TDbContext> GetDbContextAsync(CancellationToken ct) {
        if (_dbContext != null) return _dbContext;

        await _initLockAsync.WaitAsync(ct);
        try {
            // Double-check pattern
            if (_dbContext != null) return _dbContext;

            _dbContext = await dbContextFactory.CreateDbContextAsync(ct);
            return _dbContext;
        }
        finally {
            _initLockAsync.Release();
        }
    }

    public virtual TDbContext GetDbContext() => _lazyDb.Value;

    public virtual bool TryCreateTransaction() {
        if (_transaction != null) return false;
        
        var dbContext = GetDbContext<TDbContext>();
        if (dbContext.Database.CurrentTransaction != null) {
            _transaction = dbContext.Database.CurrentTransaction;
            return true;
        }

        lock (_transactionLock) {
            _transaction = dbContext.Database.BeginTransaction();
            return true;
        }
    }
    
    public virtual async ValueTask<bool> TryCreateTransactionAsync(CancellationToken ct = default) {
        if (_transaction != null) return false;

        TDbContext dbContext = await GetDbContextAsync(ct);
        if (dbContext.Database.CurrentTransaction != null) {
            _transaction = dbContext.Database.CurrentTransaction;
            return true;
        }

        await _transactionLockAsync.WaitAsync(ct);
        try {
            _transaction = await dbContext.Database.BeginTransactionAsync(ct);
            return true;
        }
        finally {
            _transactionLockAsync.Release();
        }
    }

    public virtual void SaveChanges() {
        DbContext dbContext = GetDbContext<TDbContext>();
        dbContext.SaveChanges();
    }
    
    public virtual async ValueTask SaveChangesAsync(CancellationToken ct = default) {
        DbContext dbContext = await GetDbContextAsync(ct);
        await dbContext.SaveChangesAsync(ct);
    }
    
    public virtual bool TryCommitTransaction() {
        if (_transaction == null) return false;

        lock (_transactionLock) {
            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;
            return true;
        }
    }

    public virtual async ValueTask<bool> TryCommitTransactionAsync(CancellationToken ct = default) {
        if (_transaction == null) return false;

        await _transactionLockAsync.WaitAsync(ct);
        try {
            await _transaction.CommitAsync(ct);
            await _transaction.DisposeAsync();
            _transaction = null;
            return true;
        }
        finally {
            _transactionLockAsync.Release();
        }
    }

    public virtual bool TryRollbackTransaction() {
        if (_transaction == null) return false;
        
        _transaction.Rollback();
        _transaction.Dispose();
        _transaction = null;
        
        return true;
    }

    public virtual async ValueTask<bool> TryRollbackTransactionAsync(CancellationToken ct = default) {
        if (_transaction == null) return false;

        await _transaction.RollbackAsync(ct);
        await _transaction.DisposeAsync();
        _transaction = null;

        return true;
    }

    public virtual bool TryRollbackToSavepoint(Guid id) {
        if (_transaction == null) return false;
        if (!_transaction.SupportsSavepoints) return false;
        
        _transaction.RollbackToSavepoint(id.ToString("N"));
        return true;
    }

    public virtual async ValueTask<bool> TryRollbackToSavepointAsync(Guid id, CancellationToken ct = default) {
        if (_transaction == null) return false;
        if (!_transaction.SupportsSavepoints) return false;

        await _transaction.RollbackToSavepointAsync(id.ToString("N"), ct);

        return true;
    }
    
    public virtual bool TryCreateSavepoint(Guid id) {
        if (_transaction == null) return false;
        if (!_transaction.SupportsSavepoints) return false;
        
        _transaction.CreateSavepoint(id.ToString("N"));
        return true;
    }

    public virtual async ValueTask<bool> TryCreateSavepointAsync(Guid id, CancellationToken ct = default) {
        if (_transaction == null) return false;
        if (!_transaction.SupportsSavepoints) return false;

        await _transaction.CreateSavepointAsync(id.ToString("N"), ct);

        return true;
    }
    
    public virtual T GetDbContext<T>() where T : DbContext {
        if (typeof(T) != typeof(TDbContext)) throw new NotSupportedException($"DbContext type '{typeof(T)}' is not supported by this UnitOfWork.");
        
        TDbContext dbContext = GetDbContext();
        
        return Unsafe.As<TDbContext, T>(ref dbContext);
    }

    public virtual async ValueTask<T> GetDbContextAsync<T>(CancellationToken ct = default) where T : DbContext {
        if (typeof(T) != typeof(TDbContext)) throw new NotSupportedException($"DbContext type '{typeof(T)}' is not supported by this UnitOfWork.");

        TDbContext dbContext = await GetDbContextAsync(ct);
        
        return Unsafe.As<TDbContext, T>(ref dbContext);
    }

    public virtual TRepo GetRepository<TRepo>() where TRepo : class, IUnitOfWorkRepository {
        if (AttachedRepositories.TryGetValue(typeof(TRepo), out IUnitOfWorkRepository? cachedRepo) && cachedRepo is TRepo castedCachedRepo) return castedCachedRepo;
        
        var repo = CreateAndAttachRepository<TRepo>();
        
        AttachedRepositories.AddOrUpdate(
            typeof(TRepo),
            repo, 
            (_, _) => repo
        );
        return repo;
    }

    public virtual async ValueTask<TRepo> GetRepositoryAsync<TRepo>(CancellationToken ct = default) where TRepo : class, IUnitOfWorkRepository {
        if (AttachedRepositories.TryGetValue(typeof(TRepo), out IUnitOfWorkRepository? cachedRepo) && cachedRepo is TRepo castedCachedRepo) return castedCachedRepo;

        // Cache miss so we create a new instance
        var repo = await CreateAndAttachRepositoryAsync<TRepo>(ct);

        AttachedRepositories.AddOrUpdate(
            typeof(TRepo),
            repo, 
            (_, _) => repo
        );
        return repo;
    }

    private TRepo CreateAndAttachRepository<TRepo>() where TRepo : class, IUnitOfWorkRepository {
        var repo = serviceScope.ServiceProvider.GetRequiredService<TRepo>();
        if (repo is not UnitOfWorkRepository<TDbContext> castedRepo) throw new InvalidCastException($"Cannot cast repository of type '{repo.GetType()}' to '{typeof(TRepo)}'");
        
        castedRepo.Attach(this);
        return repo;
    }
    
    private async ValueTask<TRepo> CreateAndAttachRepositoryAsync<TRepo>(CancellationToken ct = default) where TRepo : class, IUnitOfWorkRepository {
        var repo = serviceScope.ServiceProvider.GetRequiredService<TRepo>();
        if (repo is not UnitOfWorkRepository<TDbContext> castedRepo) throw new InvalidCastException($"Cannot cast repository of type '{repo.GetType()}' to '{typeof(TRepo)}'");

        await castedRepo.AttachAsync(this, ct);
        return repo;
    }

    public virtual async ValueTask DisposeAsync() {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (_initLockAsync == null) {
            GC.SuppressFinalize(this);
            IsDisposed = true;
            return;
        }

        await _initLockAsync.WaitAsync();
        try {
            if (_transaction != null) {
                await TryRollbackTransactionAsync();
            }

            foreach (IUnitOfWorkRepository repository in AttachedRepositories.Values) {
                if (repository is UnitOfWorkRepository<TDbContext> castedRepo) {
                    castedRepo.Detach();
                }
            }

            AttachedRepositories.Clear();

            if (_dbContext != null) {
                await _dbContext.DisposeAsync();
                _dbContext = null;
            }

            await serviceScope.DisposeAsync();
        }
        finally {
            _initLockAsync.Release();
            _initLockAsync.Dispose();
            GC.SuppressFinalize(this);
        }
        IsDisposed = true;
    }

}
