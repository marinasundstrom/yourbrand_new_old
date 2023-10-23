using Catalog.API.Domain.Entities;
using Catalog.API.Domain.Enums;

using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Persistence;

public static class Seed2
{
    private static string cdnBaseUrl;
    private static ProductCategory drinks;
    private static ProductCategory food;
    private static ProductCategory tshirts;
    private static ProductCategory clothes;

    public static async Task SeedData(CatalogContext context, IConfiguration configuration)
    {
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        var connectionString = context.Database.GetConnectionString()!;

        cdnBaseUrl = (connectionString.Contains("localhost") || connectionString.Contains("mssql"))
            ? configuration["CdnBaseUrl"]!
            : "https://yourbrandstorage.blob.core.windows.net";

        var currency = context.Currencies.Add(new Currency("SEK", "Swedish Krona", "kr")).Entity;

        await context.SaveChangesAsync();

        context.Stores.Add(new Store("My store", "my-store", currency));

        await context.SaveChangesAsync();

        context.Brands.Add(new Brand("my-brand", "my-brand"));

        await context.SaveChangesAsync();

        clothes = new ProductCategory("Clothes")
        {
            Handle = "clothes",
            Path = "clothes",
            Description = null,
            Store = await context.Stores.FirstAsync(x => x.Handle == "my-store")
        };

        context.ProductCategories.Add(clothes);

        tshirts = new ProductCategory("T-shirts")
        {
            Handle = "t-shirts",
            Path = "t-shirts",
            Description = null,
            CanAddProducts = true,
            Store = await context.Stores.FirstAsync(x => x.Handle == "my-store"),
        };

        clothes.AddSubCategory(tshirts);

        context.ProductCategories.Add(tshirts);

        food = new ProductCategory("Food")
        {
            Handle = "food",
            Path = "food",
            Description = null,
            CanAddProducts = true,
            Store = await context.Stores.FirstAsync(x => x.Handle == "my-store")
        };

        context.ProductCategories.Add(food);

        drinks = new ProductCategory("Drinks")
        {
            Handle = "drinks",
            Path = "drinks",
            Description = null,
            CanAddProducts = true,
            Store = await context.Stores.FirstAsync(x => x.Handle == "my-store")
        };

        context.ProductCategories.Add(drinks);

        await context.SaveChangesAsync();

        await CreateTShirt(context);

        await CreateKebabPlate(context);

        await CreateHerrgardsStek(context);

        await CreateKorg(context);

        await CreatePizza(context);

        await CreateSalad(context);
    }

    public static async Task CreateTShirt(CatalogContext context)
    {
        var sizeAttribute = new Domain.Entities.Attribute("Size");

        context.Attributes.Add(sizeAttribute);
        var valueSmall = new AttributeValue("Small");

        sizeAttribute.Values.Add(valueSmall);
        var valueMedium = new AttributeValue("Medium");

        sizeAttribute.Values.Add(valueMedium);
        var valueLarge = new AttributeValue("Large");

        sizeAttribute.Values.Add(valueLarge);
        context.Attributes.Add(sizeAttribute);

        var colorAttribute = new Domain.Entities.Attribute("Color");
        context.Attributes.Add(colorAttribute);

        var valueBlue = new AttributeValue("Blue");
        colorAttribute.Values.Add(valueBlue);

        var valueRed = new AttributeValue("Red");
        colorAttribute.Values.Add(valueRed);

        var item = new Product("Färgad t-shirt", "fargad-tshirt")
        {
            Description = "",
            Headline = "T-shirt i olika färger",
            HasVariants = true,
            Visibility = ProductVisibility.Listed,
            Brand = await context.Brands.FirstAsync(x => x.Handle == "my-brand"),
            Store = await context.Stores.FirstAsync(x => x.Handle == "my-store"),
            Image = $"{cdnBaseUrl}/images/products/placeholder.jpeg",
        };

        tshirts.AddProduct(item);

        context.Products.Add(item);

        item.AddProductAttribute(new ProductAttribute
        {
            ForVariant = true,
            IsMainAttribute = true,
            Attribute = colorAttribute,
            Value = null
        });

        item.AddProductAttribute(new ProductAttribute
        {
            ForVariant = true,
            IsMainAttribute = false,
            Attribute = sizeAttribute,
            Value = null
        });

        var variantBlueSmall = new Product("Blue S", "tshirt-blue-small")
        {
            Description = "",
            GTIN = "4345547457457",
            Price = 120,
            Store = await context.Stores.FirstAsync(x => x.Handle == "my-store"),
            Image = $"{cdnBaseUrl}/images/products/placeholder.jpeg",
        };

        variantBlueSmall.AddProductAttribute(new ProductAttribute
        {
            Attribute = sizeAttribute,
            Value = valueSmall,
            ForVariant = true
        });

        variantBlueSmall.AddProductAttribute(new ProductAttribute
        {
            Attribute = colorAttribute,
            Value = valueBlue,
            ForVariant = true,
            IsMainAttribute = true
        });

        item.AddVariant(variantBlueSmall);

        //*/

        var variantBlueMedium = new Product("Blue M", "tshirt-blue-medium")
        {
            Description = "",
            GTIN = "543453454567",
            Price = 120,
            Store = await context.Stores.FirstAsync(x => x.Handle == "my-store"),
            Image = $"{cdnBaseUrl}/images/products/placeholder.jpeg",
        };

        variantBlueMedium.AddProductAttribute(new ProductAttribute
        {
            Attribute = sizeAttribute,
            Value = valueMedium,
            ForVariant = true
        });

        variantBlueMedium.AddProductAttribute(new ProductAttribute
        {
            Attribute = colorAttribute,
            Value = valueBlue,
            ForVariant = true,
            IsMainAttribute = true
        });

        item.AddVariant(variantBlueMedium);

        var variantBlueLarge = new Product("Blue L", "tshirt-blue-large")
        {
            Description = "",
            GTIN = "6876345345345",
            Price = 60,
            Store = await context.Stores.FirstAsync(x => x.Handle == "my-store"),
            Image = $"{cdnBaseUrl}/images/products/placeholder.jpeg",
        };

        variantBlueLarge.AddProductAttribute(new ProductAttribute
        {
            Attribute = sizeAttribute,
            Value = valueLarge,
            ForVariant = true
        });

        variantBlueLarge.AddProductAttribute(new ProductAttribute
        {
            Attribute = colorAttribute,
            Value = valueBlue,
            ForVariant = true,
            IsMainAttribute = true
        });

        item.AddVariant(variantBlueLarge);

        /////

        var variantRedSmall = new Product("Red S", "tshirt-red-small")
        {
            Description = "",
            GTIN = "4345547457457",
            Price = 120,
            Store = await context.Stores.FirstAsync(x => x.Handle == "my-store"),
            Image = $"{cdnBaseUrl}/images/products/placeholder.jpeg",
        };

        variantRedSmall.AddProductAttribute(new ProductAttribute
        {
            Attribute = sizeAttribute,
            Value = valueSmall,
            ForVariant = true
        });

        variantRedSmall.AddProductAttribute(new ProductAttribute
        {
            Attribute = colorAttribute,
            Value = valueRed,
            ForVariant = true,
            IsMainAttribute = true
        });

        item.AddVariant(variantRedSmall);

        var variantRedMedium = new Product("Red M", "tshirt-red-medium")
        {
            Description = "",
            GTIN = "543453454567",
            Price = 120,
            Store = await context.Stores.FirstAsync(x => x.Handle == "my-store"),
            Image = $"{cdnBaseUrl}/images/products/placeholder.jpeg",
        };

        variantRedMedium.AddProductAttribute(new ProductAttribute
        {
            Attribute = sizeAttribute,
            Value = valueMedium,
            ForVariant = true,
        });

        variantRedMedium.AddProductAttribute(new ProductAttribute
        {
            Attribute = colorAttribute,
            Value = valueRed,
            ForVariant = true,
            IsMainAttribute = true
        });

        item.AddVariant(variantRedMedium);

        var variantRedLarge = new Product("Red L", "tshirt-red-large")
        {
            Description = "",
            GTIN = "6876345345345",
            Price = 120,
            Store = await context.Stores.FirstAsync(x => x.Handle == "my-store"),
            Image = $"{cdnBaseUrl}/images/products/placeholder.jpeg",
        };

        variantRedLarge.AddProductAttribute(new ProductAttribute
        {
            Attribute = sizeAttribute,
            Value = valueLarge,
            ForVariant = true
        });

        variantRedLarge.AddProductAttribute(new ProductAttribute
        {
            Attribute = colorAttribute,
            Value = valueRed,
            ForVariant = true,
            IsMainAttribute = true
        });

        item.AddVariant(variantRedLarge);

        var textOption = new Domain.Entities.TextValueOption("Custom text");

        item.AddOption(textOption);

        await context.SaveChangesAsync();
    }

    public static async Task CreateKebabPlate(CatalogContext context)
    {
        var item = new Product("Kebabtallrik", "kebabtallrik")
        {
            Description = "",
            Headline = "Dönnerkebab, nyfriterad pommes frites, sallad, och sås",
            Price = 89,
            Store = await context.Stores.FirstAsync(x => x.Handle == "my-store"),
            Image = $"{cdnBaseUrl}/images/products/placeholder.jpeg",
        };

        food.AddProduct(item);

        context.Products.Add(item);

        await context.SaveChangesAsync();

        var option = new ChoiceOption("Sås");
        item.AddOption(option);

        await context.SaveChangesAsync();

        var valueSmall = new OptionValue("Mild sås");

        option.Values.Add(valueSmall);

        var valueMedium = new OptionValue("Stark sås");

        option.Values.Add(valueMedium);

        var valueLarge = new OptionValue("Blandad sås");

        option.DefaultValue = valueSmall;

        option.Values.Add(valueLarge);

        await context.SaveChangesAsync();
    }

    public static async Task CreateHerrgardsStek(CatalogContext context)
    {
        var item = new Product("Herrgårdsstek", "herrgardsstek")
        {
            Description = "",
            Headline = "Vår fina stek med pommes och vår hemlagade bearnaise sås",
            Price = 179,
            Store = await context.Stores.FirstAsync(x => x.Handle == "my-store"),
            Image = $"{cdnBaseUrl}/images/products/placeholder.jpeg",
        };

        food.AddProduct(item);

        context.Products.Add(item);

        await context.SaveChangesAsync();

        var optionDoneness = new ChoiceOption("Stekning");

        item.AddOption(optionDoneness);

        await context.SaveChangesAsync();

        optionDoneness.Values.Add(new OptionValue("Rare")
        {
            Seq = 1
        });

        var optionMediumRare = new OptionValue("Medium Rare")
        {
            Seq = 2
        };

        optionDoneness.Values.Add(optionMediumRare);

        optionDoneness.Values.Add(new OptionValue("Well Done")
        {
            Seq = 3
        });

        optionDoneness.DefaultValue = optionMediumRare;

        var optionSize = new SelectableOption("Extra stor - 50 g mer")
        {
            Price = 15
        };

        item.AddOption(optionSize);

        await context.SaveChangesAsync();
    }

    public static async Task CreateKorg(CatalogContext context)
    {
        var item = new Product("Korg", "korg")
        {
            Description = "",
            Headline = "En korg med smårätter",
            Price = 179,
            Store = await context.Stores.FirstAsync(x => x.Handle == "my-store"),
            Image = $"{cdnBaseUrl}/images/products/placeholder.jpeg",
        };

        food.AddProduct(item);

        context.Products.Add(item);

        await context.SaveChangesAsync();

        var ratterGroup = new OptionGroup("Rätter")
        {
            Max = 7
        };

        item.AddOptionGroup(ratterGroup);

        var optionFalafel = new NumericalValueOption("Falafel")
        {
            Group = ratterGroup
        };

        item.AddOption(optionFalafel);

        var optionChickenWing = new NumericalValueOption("Spicy Chicken Wing")
        {
            Group = ratterGroup
        };

        item.AddOption(optionChickenWing);

        var optionRib = new NumericalValueOption("Rib")
        {
            Group = ratterGroup
        };

        item.AddOption(optionRib);

        await context.SaveChangesAsync();


        var extraGroup = new OptionGroup("Extra");

        item.AddOptionGroup(extraGroup);

        await context.SaveChangesAsync();

        var optionSauce = new ChoiceOption("Sås")
        {
            Group = extraGroup
        };

        item.AddOption(optionSauce);

        optionSauce.Values.Add(new OptionValue("Favoritsås")
        {
            Price = 10
        });

        optionSauce.Values.Add(new OptionValue("Barbecuesås")
        {
            Price = 10
        });

        await context.SaveChangesAsync();
    }

    public static async Task CreatePizza(CatalogContext context)
    {
        var item = new Product("Pizza", "pizza")
        {
            Description = "",
            Headline = "Custom pizza",
            Price = 40,
            Store = await context.Stores.FirstAsync(x => x.Handle == "my-store"),
            Image = $"{cdnBaseUrl}/images/products/placeholder.jpeg",
        };

        food.AddProduct(item);

        context.Products.Add(item);

        await context.SaveChangesAsync();

        var breadGroup = new OptionGroup("Meat")
        {
            Seq = 1,
        };

        item.AddOptionGroup(breadGroup);

        var meatGroup = new OptionGroup("Meat")
        {
            Seq = 2,
            Max = 2
        };

        item.AddOptionGroup(meatGroup);

        var nonMeatGroup = new OptionGroup("Non-Meat")
        {
            Seq = 3
        };

        item.AddOptionGroup(nonMeatGroup);

        var sauceGroup = new OptionGroup("Sauce")
        {
            Seq = 4
        };

        item.AddOptionGroup(sauceGroup);

        var toppingsGroup = new OptionGroup("Toppings")
        {
            Seq = 5
        };

        item.AddOptionGroup(toppingsGroup);

        await context.SaveChangesAsync();

        var optionStyle = new ChoiceOption("Style");

        item.AddOption(optionStyle);

        await context.SaveChangesAsync();

        var valueItalian = new OptionValue("Italian");

        optionStyle.Values.Add(valueItalian);

        var valueAmerican = new OptionValue("American");

        optionStyle.DefaultValue = valueAmerican;

        optionStyle.Values.Add(valueAmerican);

        var optionHam = new SelectableOption("Ham")
        {
            Group = meatGroup,
            Price = 15
        };

        item.AddOption(optionHam);

        var optionKebab = new SelectableOption("Kebab")
        {
            Group = meatGroup,
            Price = 10,
            IsSelected = true
        };

        item.AddOption(optionKebab);

        var optionChicken = new SelectableOption("Chicken")
        {
            Group = meatGroup,
            Price = 10
        };

        item.AddOption(optionChicken);

        var optionExtraCheese = new SelectableOption("Extra cheese")
        {
            Group = toppingsGroup,
            Price = 5
        };

        item.AddOption(optionExtraCheese);

        var optionGreenOlives = new SelectableOption("Green Olives")
        {
            Group = toppingsGroup,
            Price = 5
        };

        item.AddOption(optionGreenOlives);

        await context.SaveChangesAsync();
    }

    public static async Task CreateSalad(CatalogContext context)
    {
        var item = new Product("Sallad", "sallad")
        {
            Description = "",
            Headline = "Din egna sallad",
            Price = 52,
            Visibility = ProductVisibility.Listed,
            Store = await context.Stores.FirstAsync(x => x.Handle == "my-store"),
            Image = $"{cdnBaseUrl}/images/products/placeholder.jpeg",
        };

        food.AddProduct(item);

        context.Products.Add(item);

        var baseGroup = new OptionGroup("Bas")
        {
            Seq = 1,
        };

        item.AddOptionGroup(baseGroup);

        var proteinGroup = new OptionGroup("Välj protein")
        {
            Seq = 2,
            Max = 1
        };

        item.AddOptionGroup(proteinGroup);

        var additionalGroup = new OptionGroup("Välj tillbehör")
        {
            Seq = 4,
            Max = 3
        };

        item.AddOptionGroup(additionalGroup);

        var dressingGroup = new OptionGroup("Välj dressing")
        {
            Seq = 5,
            Max = 1
        };

        item.AddOptionGroup(dressingGroup);

        await context.SaveChangesAsync();

        var optionBase = new ChoiceOption("Bas")
        {
            Group = baseGroup
        };

        item.AddOption(optionBase);

        await context.SaveChangesAsync();

        var valueSallad = new OptionValue("Sallad");

        optionBase.Values.Add(valueSallad);

        var valueSalladPasta = new OptionValue("Sallad med pasta");

        optionBase.DefaultValue = valueSalladPasta;

        optionBase.Values.Add(valueSalladPasta);

        var valueSalladQuinoa = new OptionValue("Sallad med quinoa");

        optionBase.Values.Add(valueSalladQuinoa);

        var valueSalladNudlar = new OptionValue("Sallad med glasnudlar");

        optionBase.Values.Add(valueSalladNudlar);

        var optionChicken = new SelectableOption("Kycklingfilé")
        {
            Group = proteinGroup
        };

        item.AddOption(optionChicken);

        var optionSmokedTurkey = new SelectableOption("Rökt kalkonfilé")
        {
            Group = proteinGroup
        };

        item.AddOption(optionSmokedTurkey);

        var optionBeanMix = new SelectableOption("Marinerad bönmix")
        {
            Group = proteinGroup
        };

        item.AddOption(optionBeanMix);

        var optionVegMe = new SelectableOption("VegMe")
        {
            Group = proteinGroup
        };

        item.AddOption(optionVegMe);

        var optionChevre = new SelectableOption("Chevré")
        {
            Group = proteinGroup
        };

        item.AddOption(optionChevre);

        var optionSmokedSalmon = new SelectableOption("Varmrökt lax")
        {
            Group = proteinGroup
        };

        item.AddOption(optionSmokedSalmon);

        var optionPrawns = new SelectableOption("Handskalade räkor")
        {
            Group = proteinGroup
        };

        item.AddOption(optionPrawns);

        var optionCheese = new SelectableOption("Parmesanost")
        {
            Group = additionalGroup
        };

        item.AddOption(optionCheese);

        var optionGreenOlives = new SelectableOption("Gröna oliver")
        {
            Group = additionalGroup
        };

        item.AddOption(optionGreenOlives);

        var optionSoltorkadTomat = new SelectableOption("Soltorkade tomater")
        {
            Group = additionalGroup
        };

        item.AddOption(optionSoltorkadTomat);

        var optionInlagdRödlök = new SelectableOption("Inlagd rödlök")
        {
            Group = additionalGroup
        };

        item.AddOption(optionInlagdRödlök);

        var optionRostadAioli = new SelectableOption("Rostad aioli")
        {
            Group = dressingGroup
        };

        item.AddOption(optionRostadAioli);

        var optionPesto = new SelectableOption("Pesto")
        {
            Group = dressingGroup
        };

        item.AddOption(optionPesto);

        var optionOrtvinagret = new SelectableOption("Örtvinägrett")
        {
            Group = dressingGroup
        };

        item.AddOption(optionOrtvinagret);

        var optionSoyavinagret = new SelectableOption("Soyavinägrett")
        {
            Group = dressingGroup
        };

        item.AddOption(optionSoyavinagret);

        var optionRhodeIsland = new SelectableOption("Rhode Island")
        {
            Group = dressingGroup
        };

        item.AddOption(optionRhodeIsland);

        var optionKimchimayo = new SelectableOption("Kimchimayo")
        {
            Group = dressingGroup
        };

        item.AddOption(optionKimchimayo);

        var optionCaesar = new SelectableOption("Caesar")
        {
            Group = dressingGroup
        };

        item.AddOption(optionCaesar);

        var optionCitronLime = new SelectableOption("Citronlime")
        {
            Group = dressingGroup
        };

        item.AddOption(optionCitronLime);

        await context.SaveChangesAsync();
    }
}