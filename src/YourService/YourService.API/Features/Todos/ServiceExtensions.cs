namespace YourBrand.YourService.API.Features.Todos;

public static class ServiceExtensions
{
    public static IServiceCollection AddTodoFeature(this IServiceCollection services)
    {
        services.AddScoped<ITodosClient, TodosClient>();

        return services;
    }
}