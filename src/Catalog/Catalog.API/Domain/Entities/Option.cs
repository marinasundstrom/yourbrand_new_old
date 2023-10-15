﻿namespace Catalog.API.Domain.Entities;

using System.ComponentModel.DataAnnotations.Schema;

using Catalog.API.Domain.Enums;

public abstract class Option : Entity<string>
{
    protected Option() { }

    public Option(string name)
        : base(Guid.NewGuid().ToString())
    {
        Name = name;
    }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public OptionGroup? Group { get; set; }

    public ProductCategory? ProductCategory { get; set; }

    public OptionType OptionType { get; protected set; }

    public bool IsRequired { get; set; }

    public List<Product> Products { get; } = new List<Product>();

    public List<ProductVariantOption> ProductVariantOptions { get; } = new List<ProductVariantOption>();
}

public sealed class SelectableOption : Option
{
    private SelectableOption() { }

    public SelectableOption(string name)
        : base(name)
    {
        OptionType = OptionType.YesOrNo;
    }

    public bool IsSelected { get; set; }

    [Column("InventoryProductId")]
    public string? SKU { get; set; }

    public decimal? Price { get; set; }
}

public sealed class ChoiceOption : Option
{
    private ChoiceOption() { }

    public ChoiceOption(string name)
        : base(name)
    {
        OptionType = OptionType.Choice;
    }

    public List<OptionValue> Values { get; } = new List<OptionValue>();

    [ForeignKey(nameof(DefaultValue))]
    public string? DefaultValueId { get; set; }

    public OptionValue? DefaultValue { get; set; }
}

public sealed class NumericalValueOption : Option
{
    private NumericalValueOption() { }

    public NumericalValueOption(string name)
        : base(name)
    {
        OptionType = OptionType.NumericalValue;
    }

    public int? MinNumericalValue { get; set; }

    public int? MaxNumericalValue { get; set; }

    public int? DefaultNumericalValue { get; set; }
}

public sealed class TextValueOption : Option
{
    private TextValueOption() { }

    public TextValueOption(string name)
        : base(name)
    {
        OptionType = OptionType.TextValue;
    }

    public int? TextValueMinLength { get; set; }

    public int? TextValueMaxLength { get; set; }

    public string? DefaultTextValue { get; set; }
}
