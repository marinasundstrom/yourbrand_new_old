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

        var pastries = new ProductCategory() {
            Name = "Pastries",
            Description = "Try some of our tasty pastries.",
            Handle = "pastries",
            Path = "pastries",
            CanAddProducts = true
        };

        context.ProductCategories.Add(pastries);

        var drinks = new ProductCategory() {
            Name = "Drinks",
            Description = "All of our drinks.",
            Handle = "drinks",
            Path = "drinks"
        };

        context.ProductCategories.Add(drinks);

        var coffee = new ProductCategory() {
            Name = "Coffee",
            Description = "Enjoy your favorite coffee.",
            Handle = "coffee",
            CanAddProducts = true
        };

        drinks.AddSubCategory(coffee);

        context.ProductCategories.Add(coffee);

        var otherDrinks = new ProductCategory() {
            Name = "Other drinks",
            Description = "Try some of our special drinks.",
            Handle = "other",
            CanAddProducts = true
        };

        drinks.AddSubCategory(otherDrinks);

        context.ProductCategories.Add(otherDrinks);

        await context.SaveChangesAsync();

        var biscotti = new Product() {
            Name = "Biscotti",
            Description = "Small biscuit",
            Price = 10,
            RegularPrice = null,
            Image = $"{cdnBaseUrl}/images/products/biscotti.jpeg",
            Handle = "biscotti"
        };

        pastries.AddProduct(biscotti);

        context.Products.Add(biscotti);

        var signatureBrewed = new Product() {
            Name = "Signature Brew",
            Description = "Freshly brewed coffee",
            Price = 32,
            RegularPrice = null,
            Image = $"{cdnBaseUrl}/images/products/coffee.jpeg",
            Handle = "brewed-coffe"
        };

        coffee.AddProduct(signatureBrewed);

        context.Products.Add(signatureBrewed);

        var caffeLatte = new Product() 
        {
            Name = "Caffe Latte",
            Description = "Freshly ground espresso coffee with steamed milk",
            Price = 32,
            RegularPrice = 42,
            Image = $"{cdnBaseUrl}/images/products/caffe-latte.jpeg",
            Handle = "caffe-latte"
        };

        coffee.AddProduct(caffeLatte);

        context.Products.Add(caffeLatte);

        var cinnamonRoll = new Product() 
        {
            Name = "Cinnamon roll",
            Description = "Newly baked cinnamon rolls",
            Price = 22,
            RegularPrice = null,
            Image = $"{cdnBaseUrl}/images/products/cinnamon-roll.jpeg",
            Handle = "cinnamon-roll"
        };

        pastries.AddProduct(cinnamonRoll);

        context.Products.Add(cinnamonRoll);

        var espresso = new Product() 
        {
            Name = "Espresso",
            Description = "Single shot espresso",
            Price = 32,
            RegularPrice = null,
            Image = $"{cdnBaseUrl}/images/products/espresso.jpeg",
            Handle = "espresso"
        };

        coffee.AddProduct(espresso);

        context.Products.Add(espresso);

        var milkshake = new Product() 
        {
            Name = "Milkshake",
            Description = "Our fabulous milkshake",
            Price = 52,
            RegularPrice = null,
            Image = $"{cdnBaseUrl}/images/products/milkshake.jpeg",
            Handle = "milkshake"
        };

        otherDrinks.AddProduct(milkshake);

        context.Products.Add(milkshake);

        var moccaLatte = new Product() 
        {
            Name = "Mocca Latte",
            Description = "Caffe Latte with chocolate syrup",
            Price = 32,
            RegularPrice = null,
            Image = $"{cdnBaseUrl}/images/products/mocca-latte.jpeg",
            Handle = "mocca-latte"
        };

        coffee.AddProduct(moccaLatte);

        context.Products.Add(moccaLatte);

        var applePie = new Product() 
        {
            Name = "Apple Pie",
            Description = "Test",
            Price = 15,
            RegularPrice = null,
            Image = $"{cdnBaseUrl}/images/products/apple-pie.jpeg",
            Handle = "apple-pie"
        };

        pastries.AddProduct(applePie);

        context.Products.Add(applePie);

        await context.SaveChangesAsync();
    }
}