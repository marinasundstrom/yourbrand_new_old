using Catalog.API.Domain.Entities;
using Catalog.API.Model;

using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Persistence;

public sealed class CatalogContext : DbContext
{
    public CatalogContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogContext).Assembly);
    }

    public DbSet<Product> Products { get; set; } = default!;

    public DbSet<ProductCategory> ProductCategories { get; set; } = default!;
}