namespace Sales.API.Features.OrderManagement.Domain.ValueObjects;

public record BillingDetails
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public string? SSN { get; init; }
    public required string PhoneNumber { get; init; }
    public required string Email { get; init; }
    public required Address Address { get; init; }
}