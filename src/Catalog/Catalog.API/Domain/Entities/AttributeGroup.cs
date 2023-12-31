﻿namespace YourBrand.Catalog.API.Domain.Entities;

public class AttributeGroup : Entity<string>
{
    protected AttributeGroup() { }

    public AttributeGroup(string name)
        : base(Guid.NewGuid().ToString())
    {
        Name = name;
    }

    public int? Seq { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public Product? Product { get; set; }

    public List<Entities.Attribute> Attributes { get; } = new List<Entities.Attribute>();
}