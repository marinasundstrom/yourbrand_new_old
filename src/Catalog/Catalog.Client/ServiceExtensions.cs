namespace Catalog;

using CatalogAPI;

using Microsoft.Extensions.DependencyInjection;

public static class ServiceExtensions
{
    public static IServiceCollection AddCatalogClients(this IServiceCollection services, Uri baseUrl, Action<IHttpClientBuilder>? configureBuilder = null)
    {
        services.AddCatalogClients((sp, http) =>
        {
            http.BaseAddress = baseUrl;
        }, configureBuilder);

        return services;
    }

    public static IServiceCollection AddCatalogClients(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder>? configureBuilder = null)
    {
        IHttpClientBuilder builder = services.AddHttpClient("CatalogAPI", configureClient);

        configureBuilder?.Invoke(builder);

        services.AddHttpClient<IProductsClient>("CatalogAPI")
            .AddTypedClient<IProductsClient>((http, sp) => new CatalogAPI.ProductsClient(http));

        services.AddHttpClient<IProductCategoriesClient>("CatalogAPI")
            .AddTypedClient<IProductCategoriesClient>((http, sp) => new CatalogAPI.ProductCategoriesClient(http));

        services.AddHttpClient<IAttributesClient>("CatalogAPI")
            .AddTypedClient<IAttributesClient>((http, sp) => new CatalogAPI.AttributesClient(http));

        services.AddHttpClient<IProductOptionsClient>("CatalogAPI")
            .AddTypedClient<IProductOptionsClient>((http, sp) => new CatalogAPI.ProductOptionsClient(http));

        services.AddHttpClient<IOptionsClient>("CatalogAPI")
            .AddTypedClient<IOptionsClient>((http, sp) => new CatalogAPI.OptionsClient(http));

        services.AddHttpClient<IAttributesClient>("CatalogAPI")
            .AddTypedClient<IAttributesClient>((http, sp) => new CatalogAPI.AttributesClient(http));

        return services;
    }
}