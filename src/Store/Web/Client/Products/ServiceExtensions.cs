using BlazorApp.Products;
using StoreWeb;

namespace Client.Products;

public static class ServiceExtensions
{
    public static IServiceCollection AddProductsServices(this IServiceCollection services) 
    {
        services.AddSingleton<IProductsService, ProductsService>();

        services.AddHttpClient<StoreWeb.IProductsClient>("WebAPI")
        .AddTypedClient<StoreWeb.IProductsClient>((http, sp) => new StoreWeb.ProductsClient(http));

        return services;
    }
}