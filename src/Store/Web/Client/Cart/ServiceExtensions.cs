using BlazorApp.Cart;

namespace Client.Cart;

public static class ServiceExtensions
{
    public static IServiceCollection AddCartServices(this IServiceCollection services) 
    {
        services.AddSingleton<ICartService, CartService>();

        services.AddHttpClient<StoreWeb.ICartClient>("WebAPI")
        .AddTypedClient<StoreWeb.ICartClient>((http, sp) => new StoreWeb.CartClient(http));

        return services;
    }
}