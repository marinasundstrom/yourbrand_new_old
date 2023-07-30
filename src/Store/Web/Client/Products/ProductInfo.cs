namespace Client.Products;

public sealed record ProductInfo(string Name, string? ProductId, string Description, decimal Price, decimal? RegularPrice);
