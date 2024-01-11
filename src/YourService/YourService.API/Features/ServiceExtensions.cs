using YourBrand.YourService.API.Features.Todos;

namespace YourBrand.YourService.API.Features;

public static class ServiceExtensions
{
    public static IServiceCollection AddFeatures(this IServiceCollection services)
    {
        services.AddTodoFeature();

        return services;
    }
}