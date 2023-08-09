using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Catalog.API.Model;

namespace Catalog.API.Data;

public static class Seed
{
    public static async Task SeedData(CatalogContext context, IConfiguration configuration)
    {
        //await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        string cdnBaseUrl = configuration["CdnBaseUrl"] ?? "https://yourbrandstorage.blob.core.windows.net";

        context.Products.AddRange(
            new Product() {
                Name = "Biscotti",
                Description = "Small biscuit",
                Price = 10,
                RegularPrice = null,
                Image = $"{cdnBaseUrl}/images/products/biscotti.jpeg",
                Handle = "biscotti"
            },
            new Product() {
                Name = "Signature Brew",
                Description = "Freshly brewed coffee",
                Price = 32,
                RegularPrice = null,
                Image = $"{cdnBaseUrl}/images/products/coffee.jpeg",
                Handle = "brewed-coffe"
            },
            new Product() {
                Name = "Caffe Latte",
                Description = "Freshly ground espresso coffee with steamed milk",
                Price = 32,
                RegularPrice = 42,
                Image = $"{cdnBaseUrl}/images/products/caffe-latte.jpeg",
                Handle = "caffe-latte"
            },
            new Product() {
                Name = "Cinnamon roll",
                Description = "Newly baked cinnamon rolls",
                Price = 22,
                RegularPrice = null,
                Image = $"{cdnBaseUrl}/images/products/cinnamon-roll.jpeg",
                Handle = "cinnamon-roll"
            },
            new Product() {
                Name = "Espresso",
                Description = "Single shot espresso",
                Price = 32,
                RegularPrice = null,
                Image = $"{cdnBaseUrl}/images/products/espresso.jpeg",
                Handle = "espresso"
            },
            new Product() {
                Name = "Milkshake",
                Description = "Our fabulous milkshake",
                Price = 52,
                RegularPrice = null,
                Image = $"{cdnBaseUrl}/images/products/milkshake.jpeg",
                Handle = "milkshake"
            },
            new Product() {
                Name = "Mocca Latte",
                Description = "Caffe Latte with chocolate syrup",
                Price = 32,
                RegularPrice = null,
                Image = $"{cdnBaseUrl}/images/products/mocca-latte.jpeg",
                Handle = "mocca-latte"
            }
        );

        await context.SaveChangesAsync();
    }
}