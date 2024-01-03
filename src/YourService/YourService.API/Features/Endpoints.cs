using YourBrand.YourService.API.Features.OrderManagement;

namespace YourBrand.YourService.API.Features;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapFeaturesEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapOrderManagementEndpoints();

        return app;
    }
}