// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;

namespace CodeOfChaosTests.Extensions.EntityFrameworkCore.UnitOfWork.Assets;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class MockDbContext : DbContext, IReadonlyCapableDbContext {
    public bool IsReadonly { get; private set; }
    public void SetAsReadonly() => IsReadonly = true;
    
    public MockDbContext() {}
    public MockDbContext(DbContextOptions<MockDbContext> options) : base(options) {}
}
