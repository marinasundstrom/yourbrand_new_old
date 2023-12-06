namespace StoreFront;

using StoreFrontAPI;

using Microsoft.Extensions.DependencyInjection;

public static class ServiceExtensions
{
    public static IServiceCollection AddStoreFrontClients(this IServiceCollection services, Uri baseUrl, Action<IHttpClientBuilder>? configureBuilder = null)
    {
        services.AddCatalogClients((sp, http) =>
        {
            http.BaseAddress = baseUrl;
        }, configureBuilder);


        services.AddCartClient((sp, http) =>
        {
            http.BaseAddress = baseUrl;
        }, configureBuilder);

        return services;
    }

    public static IServiceCollection AddStoreFrontClients(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder>? configureBuilder = null)
    {
        services.AddCatalogClients(configureClient, configureBuilder);

        services.AddCartClient(configureClient, configureBuilder);

        return services;
    }

    public static IServiceCollection AddCatalogClients(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder>? configureBuilder = null)
    {
        IHttpClientBuilder builder = services.AddHttpClient("CatalogAPI", configureClient);

        configureBuilder?.Invoke(builder);

        services.AddHttpClient<IProductsClient>("CatalogAPI")
            .AddTypedClient<IProductsClient>((http, sp) => new StoreFrontAPI.ProductsClient(http));

        services.AddHttpClient<IProductCategoriesClient>("CatalogAPI")
            .AddTypedClient<IProductCategoriesClient>((http, sp) => new StoreFrontAPI.ProductCategoriesClient(http));

        return services;
    }

    public static IServiceCollection AddCartClient(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder>? configureBuilder = null)
    {
        IHttpClientBuilder builder = services.AddHttpClient("CartAPI", configureClient);

        configureBuilder?.Invoke(builder);

        services.AddHttpClient<ICartClient>("CartAPI")
            .AddTypedClient<ICartClient>((http, sp) => new StoreFrontAPI.CartClient(http));

        return services;
    }
}