using YourBrand.Sales.API.Features.OrderManagement.Domain;
using YourBrand.Sales.API.Features.OrderManagement.Domain.Events;

namespace YourBrand.Sales.API.Features.OrderManagement.Domain.Entities;

public class Address
{
    /*
    public Address()
    {
        Id = Guid.NewGuid().ToString();
    }
    */

    // Street
    public string Thoroughfare { get; set; } = null!;

    // Street number
    public string? Premises { get; set; }

    // Suite
    public string? SubPremises { get; set; }

    public string PostalCode { get; set; } = null!;

    // Town or City
    public string Locality { get; set; } = null!;

    // County
    public string SubAdministrativeArea { get; set; } = null!;

    // State
    public string AdministrativeArea { get; set; } = null!;

    public string Country { get; set; } = null!;

    public User? CreatedBy { get; set; }

    public string? CreatedById { get; set; }

    public DateTimeOffset Created { get; set; }

    public User? LastModifiedBy { get; set; }

    public string? LastModifiedById { get; set; }

    public DateTimeOffset? LastModified { get; set; }
}