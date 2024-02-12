using System.Linq.Expressions;

using LinqKit;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

using YourBrand.Domain.Outbox;
using YourBrand.Domain.Persistence;
using YourBrand.YourService.API.Common;
using YourBrand.YourService.API.Domain.Entities;
using YourBrand.YourService.API.Domain.ValueObjects;

namespace YourBrand.YourService.API.Infrastructure.Persistence;

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

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        ConfigQueryFilterForEntity(modelBuilder);

        //Console.WriteLine(modelBuilder.Model.ToDebugString(MetadataDebugStringOptions.LongDefault));
    }

    private void ConfigQueryFilterForEntity(ModelBuilder modelBuilder)
    {
        foreach (var clrType in modelBuilder.Model
            .GetEntityTypes()
            .Select(entityType => entityType.ClrType))
        {
            var entityTypeBuilder = modelBuilder.Entity(clrType);

            var parameter = Expression.Parameter(clrType, "entity");
            Expression? queryFilter = null;

            var softDeleteFilter = ApplySoftDeleteQueryFilter(clrType);
            if (softDeleteFilter is not null)
            {
                queryFilter = softDeleteFilter;
            }

            var tenantFilter = ConfigTenantFilter(clrType);
            if (tenantFilter is not null)
            {
                if (queryFilter is null)
                {
                    queryFilter = tenantFilter;
                }
                else
                {
                    queryFilter = Expression.AndAlso(
                        Expression.Invoke(queryFilter, Expression.Convert(parameter, typeof(ISoftDelete))),
                        Expression.Invoke(tenantFilter, Expression.Convert(parameter, typeof(IHasTenant)))).Expand();
                }
            }

            if (queryFilter is null)
            {
                continue;
            }

            var queryFilterLambda = Expression.Lambda(queryFilter, parameter);

            entityTypeBuilder.HasQueryFilter(queryFilterLambda);
        }
    }

    private Expression<Func<IHasTenant, bool>>? ConfigTenantFilter(Type entityType)
    {
        var hasTenantInterface = typeof(IHasTenant);
        if (!hasTenantInterface.IsAssignableFrom(entityType))
        {
            return null;
        }
        return (IHasTenant e) => e.TenantId == _tenantId;
    }

    private Expression<Func<ISoftDelete, bool>>? ApplySoftDeleteQueryFilter(Type entityType)
    {
        var softDeleteInterface = typeof(ISoftDelete);
        var deletedProperty = softDeleteInterface.GetProperty(nameof(ISoftDelete.Deleted));

        if (!softDeleteInterface.IsAssignableFrom(entityType))
        {
            return null;
        }

        var param = Expression.Parameter(softDeleteInterface, "entity");
        var body = Expression.Equal(Expression.Property(param, deletedProperty!), Expression.Constant(null));
        return Expression.Lambda<Func<ISoftDelete, bool>>(body, param);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<TodoId>().HaveConversion<TodoIdConverter>();
        configurationBuilder.Properties<UserId>().HaveConversion<UserIdConverter>();
        configurationBuilder.Properties<TenantId>().HaveConversion<TenantIdConverter>();
    }

#nullable disable

    public DbSet<Todo> Todos { get; set; }

    public DbSet<User> Users { get; set; }

#nullable restore
}