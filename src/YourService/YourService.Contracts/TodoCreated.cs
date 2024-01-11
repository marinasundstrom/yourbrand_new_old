namespace YourBrand.YourService.Contracts;

public sealed record TodoCreated
{
    public string TodoId { get; init; }
}