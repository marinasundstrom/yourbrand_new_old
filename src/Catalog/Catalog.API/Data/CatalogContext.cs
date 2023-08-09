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
    }

    public DbSet<Product> Products { get; set; } = default!;
}