using YourBrand.YourService.API.Features.OrderManagement.Orders;
using YourBrand.YourService.API.Features.OrderManagement.Orders.Statuses;

namespace YourBrand.YourService.API.Features.OrderManagement;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapOrderManagementEndpoints(this IEndpointRouteBuilder app)
    {
        app
            .MapOrderEndpoints()
            .MapOrderStatusEndpoints();

        return app;
    }
}