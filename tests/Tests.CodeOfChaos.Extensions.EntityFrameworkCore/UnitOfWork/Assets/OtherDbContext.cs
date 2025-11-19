// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;

namespace Tests.CodeOfChaos.Extensions.EntityFrameworkCore.UnitOfWork.Assets;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class OtherDbContext : DbContext, IReadonlyCapableDbContext {
    public bool IsReadonly { get; private set; }
    public void SetAsReadonly() => IsReadonly = true;
    
    public OtherDbContext() {}
    public OtherDbContext(DbContextOptions<OtherDbContext> options) : base(options) {}
}
