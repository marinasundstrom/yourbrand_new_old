using Catalog.API.Domain.Enums;

namespace Catalog.API.Domain.Entities;

public sealed class Product
{
    readonly List<ProductAttribute> _productAttributes = new List<ProductAttribute>();

    readonly List<AttributeGroup> _attributeGroups = new List<AttributeGroup>();

    readonly List<Product> _variants = new List<Product>();

    readonly List<Option> _options = new List<Option>();

    readonly List<ProductOption> _productOptions = new List<ProductOption>();

    readonly List<OptionGroup> _optionGroups = new List<OptionGroup>();

    readonly List<ProductVariantOption> _productVariantOptions = new List<ProductVariantOption>();

    public Product() { }

    public Product(string name, string handle)
    {
        Name = name;
        Handle = handle;
    }

    public long Id { get; private set; }

    public Store? Store { get; set; }

    public string? StoreId { get; set; }

    public Brand? Brand { get; set; }

    public string Name { get; set; } = default!;

    public ProductCategory? Category { get; set; }

    public long? CategoryId { get; set; }

    public int? BrandId { get; set; }

    public string Description { get; set; } = default!;

    public string? Headline { get; set; }

    public string? Sku { get; set; }

    public string? Gtin { get; set; }

    public decimal Price { get; set; }

    //public decimal Vat { get; set; }

    public double? VatRate { get; set; }

    public decimal? Discount { get; set; }

    public double? DiscountRate { get; set; }

    public decimal? RegularPrice { get; set; }

    public decimal? PurchasePrice { get; set; }

    public string? Image { get; set; }

    public string Handle { get; set; } = default!;

    public bool? IsCustomizable { get; set; } = false;

    public bool HasVariants { get; set; } = false;

    public Product? ParentProduct { get; set; }

    public long? ParentProductId { get; set; }

    public IReadOnlyCollection<ProductAttribute> ProductAttributes => _productAttributes;

    public IReadOnlyCollection<AttributeGroup> AttributeGroups => _attributeGroups;

    public IReadOnlyCollection<Product> Variants => _variants;

    public IReadOnlyCollection<Option> Options => _options;

    public IReadOnlyCollection<ProductOption> ProductOptions => _productOptions;

    public IReadOnlyCollection<OptionGroup> OptionGroups => _optionGroups;

    public ProductVisibility Visibility { get; set; }

    public List<ProductVariantOption> ProductVariantOptions { get; } = new List<ProductVariantOption>();

    public void AddVariant(Product variant)
    {
        _variants.Add(variant);
        variant.Category = this.Category;
    }

    public void RemoveVariant(Product variant)
    {
        _variants.Add(variant);
    }

    public void AddProductOption(ProductOption productOption)
    {
        _productOptions.Add(productOption);
    }

    public void RemoveProductOption(ProductOption option)
    {
        _productOptions.Remove(option);
    }

    public void AddProductAttribute(ProductAttribute productAttribute)
    {
        _productAttributes.Add(productAttribute);
    }

    public void RemoveProductAttribute(ProductAttribute productAttribute)
    {
        _productAttributes.Remove(productAttribute);
    }

    public void AddOptionGroup(OptionGroup group)
    {
        _optionGroups.Add(group);
    }

    public void RemoveOptionGroup(OptionGroup group)
    {
        _optionGroups.Remove(group);
    }

    public void AddOption(Option option)
    {
        _options.Add(option);
    }

    public void RemoveOption(Option option)
    {
        _options.Remove(option);
    }
}