﻿namespace Catalog.API.Domain.Entities;

public class Attribute : Entity<string>
{
    protected Attribute() { }

    public Attribute(string name)
        : base(Guid.NewGuid().ToString())
    {
        Name = name;
    }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public AttributeGroup? Group { get; set; }

    public ProductCategory? ProductCategory { get; set; }

    //public List<Product> Products { get; } = new List<Product>();

    public List<ProductAttribute> ProductAttributes { get; } = new List<ProductAttribute>();

    public List<AttributeValue> Values { get; } = new List<AttributeValue>();
}