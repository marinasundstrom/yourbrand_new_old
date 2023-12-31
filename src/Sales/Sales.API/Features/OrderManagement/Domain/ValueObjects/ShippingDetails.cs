namespace YourBrand.Sales.API.Features.OrderManagement.Domain.ValueObjects;

public record ShippingDetails
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public string? CareOf { get; init; }
    public required Address Address { get; init; }
}