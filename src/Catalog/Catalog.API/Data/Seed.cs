using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Catalog.API.Model;

namespace Catalog.API.Data;

public static class Seed
{
    public static async Task SeedData(CatalogContext context)
    {
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        context.Products.AddRange(
            new Product() {
                Id = "biscotti",
                Name = "Biscotti",
                Description = "Small biscuit",
                Price = 10,
                RegularPrice = null,
                Image = "http://127.0.0.1:10000/devstoreaccount1/images/products/biscotti.jpeg"
            }
        );

        await context.SaveChangesAsync();
    }
}