namespace YourBrand.YourService.API.Domain;

public interface IHasTenant
{
    public string TenantId { get; set; }
}