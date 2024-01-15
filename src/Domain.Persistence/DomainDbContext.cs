using Microsoft.EntityFrameworkCore;

using YourBrand.Domain.Persistence.Interceptors;

namespace YourBrand.Domain.Persistence;

public abstract class DomainDbContext : DbContext
{
    private readonly OutboxSaveChangesInterceptor _outboxSaveChangesInterceptor;

    public DomainDbContext(
        DbContextOptions options, OutboxSaveChangesInterceptor outboxSaveChangesInterceptor) : base(options)
    {
        _outboxSaveChangesInterceptor = outboxSaveChangesInterceptor;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        //optionsBuilder.AddInterceptors(_outboxSaveChangesInterceptor);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DomainDbContext).Assembly);
    }
}