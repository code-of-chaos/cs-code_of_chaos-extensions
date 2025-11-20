// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.EntityFrameworkCore;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ReadonlyUnitOfWorkFactory<TDbContext>(
    IDbContextFactory<TDbContext> dbContextFactory,
    IServiceProvider provider,
    ILoggerFactory loggerFactory
) : IReadonlyUnitOfWorkFactory<TDbContext> where TDbContext : DbContext, IReadonlyCapableDbContext {

    public IReadonlyUnitOfWork<TDbContext> Create() {
        AsyncServiceScope scope = provider.CreateAsyncScope();
        return new ReadonlyUnitOfWork<TDbContext>(dbContextFactory, scope, loggerFactory.CreateLogger<ReadonlyUnitOfWork<TDbContext>>());
    }
}
