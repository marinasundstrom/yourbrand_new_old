using Catalog.API.Domain.Enums;

namespace Catalog.API.Domain.Entities;

public sealed class Product
{
    public long Id { get; private set; }

    public string Name { get; set; } = default!;

    public ProductCategory? Category { get; set; }

    public long? CategoryId { get; set; }

    public string Description { get; set; } = default!;

    public decimal Price { get; set; }

    public decimal? RegularPrice { get; set; }

    public string? Image { get; set; }

    public string Handle { get; set; } = default!;

    public bool? IsCustomizable { get; set; } = false;

    public bool HasVariants { get; set; } = false;

    public Product? ParentProduct { get; set; }

    public long? ParentProductId { get; set; }

    public List<ProductAttribute> ProductAttributes { get; } = new List<ProductAttribute>();

    public List<AttributeGroup> AttributeGroups { get; } = new List<AttributeGroup>();

    public List<Product> Variants { get; } = new List<Product>();

    public List<Option> Options { get; } = new List<Option>();

    public List<ProductOption> ProductOptions { get; } = new List<ProductOption>();

    public List<OptionGroup> OptionGroups { get; } = new List<OptionGroup>();

    public ProductVisibility Visibility { get; set; }

    public List<ProductVariantOption> ProductVariantOptions { get; } = new List<ProductVariantOption>();
}