namespace Catalog.Contracts;

public sealed record ProductDetailsUpdated
{
    public required string ProductId { get; init; }

    public required string Name { get; init; }

    public required string Description { get; init; }
}

public sealed record ProductPriceUpdated
{
    public required string ProductId { get; init; }

    public required decimal NewPrice { get; init; }
}