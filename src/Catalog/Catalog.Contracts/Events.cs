namespace Catalog.Contracts;

public sealed record ProductDetailsUpdated
{
    public required long ProductId { get; init; }

    public required string Name { get; init; }

    public required string Description { get; init; }
}

public sealed record ProductPriceUpdated
{
    public required long ProductId { get; init; }

    public required decimal NewPrice { get; init; }
}

public sealed record ProductImageUpdated
{
    public required long ProductId { get; init; }

    public required string ImageUrl { get; init; }
}

public sealed record ProductHandleUpdated
{
    public required long ProductId { get; init; }

    public required string Handle { get; init; }
}