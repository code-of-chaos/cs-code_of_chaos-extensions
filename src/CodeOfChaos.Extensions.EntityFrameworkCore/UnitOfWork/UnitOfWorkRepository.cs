// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Concurrent;

namespace Microsoft.EntityFrameworkCore;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class UnitOfWorkRepository<TDbContext> : IUnitOfWorkRepository
    where TDbContext : DbContext {
    private TDbContext? DbContext { get; set; }
    private ConcurrentDictionary<Type,object> DbSetCache {get;} = new();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    internal void Attach(IUnitOfWork<TDbContext> unitOfWork) => DbContext = unitOfWork.GetDbContext();
    internal async ValueTask AttachAsync(IUnitOfWork<TDbContext> unitOfWork, CancellationToken ct = default) => DbContext = await unitOfWork.GetDbContextAsync(ct);
    internal void Detach() => DbContext = null;// Remove the reference to the DbContext

    protected TDbContext GetDbContext() => DbContext ?? throw new InvalidOperationException("Repository is not attached to a UnitOfWork.");
    
    protected DbSet<TModel> GetDbSet<TModel>() where TModel : class => GetDbContext().Set<TModel>();
    
    protected DbSet<TModel> GetCachedDbSet<TModel>() where TModel : class 
        => (DbSet<TModel>)DbSetCache.GetOrAdd(
            typeof(TModel),
            static (_, dbContext) => dbContext.Set<TModel>(), 
            GetDbContext()
        );
}
