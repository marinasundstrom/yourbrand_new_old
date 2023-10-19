namespace Catalog;

using CatalogAPI;

using Microsoft.Extensions.DependencyInjection;

using Steeltoe.Common.Http.Discovery;

public static class ServiceExtensions
{
    public static IServiceCollection AddCatalogClients(this IServiceCollection services, string url)
    {
        services.AddHttpClient("CatalogAPI", (sp, http) =>
        {
            http.BaseAddress = new Uri(url);
        });

        services.AddHttpClient<IProductsClient>("CatalogAPI")
            .AddServiceDiscovery()
            .AddTypedClient<IProductsClient>((http, sp) => new CatalogAPI.ProductsClient(http));

        services.AddHttpClient<IProductCategoriesClient>("CatalogAPI")
            .AddServiceDiscovery()
            .AddTypedClient<IProductCategoriesClient>((http, sp) => new CatalogAPI.ProductCategoriesClient(http));

        return services;
    }
}