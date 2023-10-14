namespace Catalog.API.Domain.Entities;

public sealed class Product 
{
    public long Id { get; private set; }

    public string Name { get; set; } = default!;

    public ProductCategory? Category { get; set; }

    public string Description { get; set; } = default!;

    public decimal Price { get; set; }

    public decimal? RegularPrice { get; set; }

    public string? Image { get; set; }

    public string Handle { get; set; } = default!;
}