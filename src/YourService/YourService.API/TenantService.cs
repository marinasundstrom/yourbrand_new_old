using YourBrand.YourService.API.Common;

namespace YourBrand.YourService.API;

public sealed class TenantService : ITenantService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TenantService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? TenantId => _httpContextAccessor?.HttpContext?.User?.FindFirst("TenantId")?.Value;
}