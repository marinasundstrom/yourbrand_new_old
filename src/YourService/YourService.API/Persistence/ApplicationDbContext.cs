using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using YourBrand.Domain.Persistence;
using YourBrand.YourService.API.Common;
using YourBrand.YourService.API.Domain.Entities;

namespace YourBrand.YourService.API.Persistence;

public sealed class ApplicationDbContext : DomainDbContext, IApplicationDbContext, IUnitOfWork
{
    private readonly string? _tenantId;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
        ITenantService tenantService)
        : base(options)
    {
        _tenantId = tenantService.TenantId;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ApplySoftDeleteQueryFilter(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        ConfigTenantFilter(modelBuilder);
    }

    private void ConfigTenantFilter(ModelBuilder modelBuilder)
    {
        /*
        modelBuilder.Entity<IHasTenant>(entity =>
        {
            entity.HasQueryFilter(e => e.TenantId == _tenantId && e.Deleted == null);
        });
        */
    }

    private static void ApplySoftDeleteQueryFilter(ModelBuilder modelBuilder)
    {
        // INFO: This code adds a query filter to any object deriving from Entity
        //       and that is implementing the ISoftDelete interface.
        //       The generated expressions correspond to: (e) => e.Deleted == null.
        //       Causing the entity not to be included in the result if Deleted is not null.
        //       There are other better ways to approach non-destructive "deletion".

        var softDeleteInterface = typeof(ISoftDelete);
        var deletedProperty = softDeleteInterface.GetProperty(nameof(ISoftDelete.Deleted));

        foreach (var entityType in softDeleteInterface.Assembly
            .GetTypes()
            .Where(candidateEntityType => candidateEntityType != typeof(ISoftDelete))
            .Where(candidateEntityType => softDeleteInterface.IsAssignableFrom(candidateEntityType)))
        {
            var param = Expression.Parameter(entityType, "entity");
            var body = Expression.Equal(Expression.Property(param, deletedProperty!), Expression.Constant(null));
            var lambda = Expression.Lambda(body, param);

            modelBuilder.Entity(entityType).HasQueryFilter(lambda);
        }
    }

#nullable disable

    public DbSet<Todo> Todos { get; set; }

    public DbSet<User> Users { get; set; }

#nullable restore
}