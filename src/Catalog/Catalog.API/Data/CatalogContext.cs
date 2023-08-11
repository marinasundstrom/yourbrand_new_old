using Catalog.API.Model;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Data;

public sealed class CatalogContext : DbContext
{
    public CatalogContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .HasIndex(p => p.Handle);

        modelBuilder.Entity<ProductCategory>()
            .HasIndex(p => p.Handle);

        modelBuilder.Entity<ProductCategory>()
            .HasIndex(p => p.Path);
    }

    public DbSet<Product> Products { get; set; } = default!;

    public DbSet<ProductCategory> ProductCategories { get; set; } = default!;
}