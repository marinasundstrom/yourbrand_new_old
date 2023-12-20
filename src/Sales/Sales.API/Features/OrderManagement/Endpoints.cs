using Sales.API.Features.OrderManagement.Orders;
using Sales.API.Features.OrderManagement.Orders.Statuses;

namespace Sales.API.Features.OrderManagement;

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