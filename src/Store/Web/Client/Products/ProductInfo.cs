namespace Client.Products;

public sealed record ProductInfo(string Name, string? Image, string? ProductId, string Description, decimal Price, decimal? RegularPrice);
