using Microsoft.EntityFrameworkCore;

namespace YourBrand.Domain.Persistence;

public abstract class DomainDbContext : DbContext
{
    public DomainDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DomainDbContext).Assembly);
    }
}