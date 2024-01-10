using YourBrand.YourService.API.Features.Todos;
using YourBrand.YourService.API.Features.Users;

namespace YourBrand.YourService.API.Features;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapFeaturesEndpoints(this IEndpointRouteBuilder app)
    {
        app
            .MapTodosEndpoints()
            .MapUsersEndpoints();

        return app;
    }
}