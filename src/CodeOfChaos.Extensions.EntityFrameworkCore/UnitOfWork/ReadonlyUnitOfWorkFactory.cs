// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ReadonlyUnitOfWorkFactory<TDbContext>(
    IDbContextFactory<TDbContext> dbContextFactory,
    IServiceProvider provider
) : IReadonlyUnitOfWorkFactory<TDbContext> where TDbContext : DbContext, IReadonlyCapableDbContext {

    public IReadonlyUnitOfWork<TDbContext> Create() {
        AsyncServiceScope scope = provider.CreateAsyncScope();
        return new ReadonlyUnitOfWork<TDbContext>(dbContextFactory, scope);
    }
}
