using Sales.API.Features.OrderManagement;

namespace Sales.API.Features;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapFeaturesEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapOrderManagementEndpoints();

        return app;
    }
}