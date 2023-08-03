namespace Carts.Contracts;

public sealed record Cart 
{
    public string Id { get; init; }
    public string Name { get; init; }
    public decimal Total { get; init; }
    public IEnumerable<CartItem> Items { get; init; }
}

public sealed record CartItem 
{
    public string Id { get; init; }
    public string Name { get; init; }
    public string? Image { get; init; }
    public string? ProductId { get; init; }
    public string Description { get; init; }
    public decimal Price { get; init; } 
    public decimal? RegularPrice { get; init; }
    public double Quantity { get; init; }
    public decimal Total { get; init; }
    public DateTimeOffset Created { get; init; }
}