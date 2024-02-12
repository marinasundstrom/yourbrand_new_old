using YourBrand.YourService.API.Domain.ValueObjects;

namespace YourBrand.YourService.API.Domain;

public interface IHasTenant
{
    public TenantId TenantId { get; set; }
}