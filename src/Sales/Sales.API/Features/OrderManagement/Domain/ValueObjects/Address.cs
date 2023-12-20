using Sales.API.Features.OrderManagement.Domain;
using Sales.API.Features.OrderManagement.Domain.Entities;
using Sales.API.Features.OrderManagement.Domain.Events;

namespace Sales.API.Features.OrderManagement.Domain.ValueObjects;

public record Address
{
    // Street
    public required string Thoroughfare { get; init; }

    // Street number
    public required string Premises { get; init; }

    // Suite
    public string? SubPremises { get; init; }

    public required string PostalCode { get; init; }

    // Town or City
    public required string Locality { get; init; }

    // County or Municipality
    public required string SubAdministrativeArea { get; init; }

    // State or Province
    public required string AdministrativeArea { get; init; }

    public required string Country { get; init; }
}