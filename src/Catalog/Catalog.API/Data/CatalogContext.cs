using Catalog.API.Model;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Data;

public sealed class CatalogContext : DbContext
{
    public CatalogContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; } = default!;
}