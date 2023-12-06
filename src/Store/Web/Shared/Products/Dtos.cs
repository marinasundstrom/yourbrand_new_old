﻿namespace BlazorApp.Products;

using BlazorApp.ProductCategories;

public sealed record Product(long Id, string Name, ProductCategoryParent? Category, string? Image, string Description, decimal Price, decimal? RegularPrice, string Handle, bool HasVariants, IEnumerable<ProductAttribute> Attributes, IEnumerable<ProductOption> Options);

public sealed record ProductAttribute(Attribute Attribute, AttributeValue? Value, bool ForVariant, bool IsMainAttribute);

public sealed record Attribute(string Id, string Name, string? Description, AttributeGroup? Group, ICollection<AttributeValue> Values);

public sealed record AttributeGroup(string Id, string Name, string? Description);

public sealed record AttributeValue(string Id, string Name, int? Seq);

public sealed record ProductOption(Option Option, bool IsInherited);

public sealed record Option(string Id, string Name, string? Description, OptionType OptionType, OptionGroup? Group, bool IsRequired, string? Sku, decimal? Price, bool? IsSelected,
    ICollection<OptionValue> Values, OptionValue? DefaultValue,
    int? MinNumericalValue, int? MaxNumericalValue, int? DefaultNumericalValue, int? TextValueMinLength, int? TextValueMaxLength, string? DefaultTextValue);

public sealed record OptionGroup(string Id, string Name, string? Description, int? Seq, int? Min, int? Max);

public enum OptionType
{
    YesOrNo = 0,

    Choice = 1,

    NumericalValue = 2,

    TextValue = 3,
}

public sealed record OptionValue(string Id, string Name, string? Sku, decimal? Price, int? Seq);