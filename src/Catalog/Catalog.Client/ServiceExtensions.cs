namespace Catalog;

using Microsoft.Extensions.DependencyInjection;
using CatalogAPI;

public static class ServiceExtensions 
{
    public static IServiceCollection AddCatalogClients(this IServiceCollection services, string url) 
    {
        services.AddHttpClient("CatalogAPI", (sp, http) =>
        {
            http.BaseAddress = new Uri(url);
        });

        services.AddHttpClient<IProductsClient>("CatalogAPI")
            .AddTypedClient<IProductsClient>((http, sp) => new CatalogAPI.ProductsClient(http));
        
        return services;
    }
}