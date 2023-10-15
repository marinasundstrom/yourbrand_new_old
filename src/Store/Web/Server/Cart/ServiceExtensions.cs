namespace BlazorApp.Cart;

public static class ServiceExtensions
{
    public static IServiceCollection AddCartServices(this IServiceCollection services)
    {
        services.AddScoped<MassTransitCartsClient>();
        services.AddScoped<ICartService, CartService>();

        return services;
    }
}