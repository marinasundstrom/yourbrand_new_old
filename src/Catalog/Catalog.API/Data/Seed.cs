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
            },
            new Product() {
                Id = "brewed-coffee",
                Name = "Signature Brew",
                Description = "Freshly brewed coffee",
                Price = 32,
                RegularPrice = null,
                Image = "http://127.0.0.1:10000/devstoreaccount1/images/products/coffee.jpeg"
            },
            new Product() {
                Id = "caffe-latte",
                Name = "Caffe Latte",
                Description = "Freshly ground espresso coffee with steamed milk",
                Price = 32,
                RegularPrice = 42,
                Image = "http://127.0.0.1:10000/devstoreaccount1/images/products/caffe-latte.jpeg"
            },
            new Product() {
                Id = "cinnamon-roll",
                Name = "Cinnamon roll",
                Description = "Newly baked cinnamon rolls",
                Price = 22,
                RegularPrice = null,
                Image = "http://127.0.0.1:10000/devstoreaccount1/images/products/cinnamon-roll.jpeg"
            },
            new Product() {
                Id = "espresso",
                Name = "Espresso",
                Description = "Single shot espresso",
                Price = 32,
                RegularPrice = null,
                Image = "http://127.0.0.1:10000/devstoreaccount1/images/products/espresso.jpeg"
            },
            new Product() {
                Id = "milkshake",
                Name = "Milkshake",
                Description = "Our fabulous milkshake",
                Price = 52,
                RegularPrice = null,
                Image = "http://127.0.0.1:10000/devstoreaccount1/images/products/milkshake.jpeg"
            },
            new Product() {
                Id = "mocca-latte",
                Name = "Mocca Latte",
                Description = "Caffe Latte with chocolate syrup",
                Price = 32,
                RegularPrice = null,
                Image = "http://127.0.0.1:10000/devstoreaccount1/images/products/mocca-latte.jpeg"
            }
        );

        await context.SaveChangesAsync();
    }
}