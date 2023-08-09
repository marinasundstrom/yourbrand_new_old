namespace Catalog.API.Model;

public sealed class Product 
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public decimal Price { get; set; }

    public decimal? RegularPrice { get; set; }

    public string? Image { get; set; }

    public string Handle { get; set; } = default!;
}