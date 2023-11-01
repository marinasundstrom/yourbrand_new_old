namespace Carts;

using CartsAPI;

using Microsoft.Extensions.DependencyInjection;

public static class ServiceExtensions
{
    public static IServiceCollection AddCartsClient(this IServiceCollection services, Uri baseUrl, Action<IHttpClientBuilder>? configureBuilder = null)
    {
        services.AddCartsClient((sp, http) =>
        {
            http.BaseAddress = baseUrl;
        }, configureBuilder);

        return services;
    }

    public static IServiceCollection AddCartsClient(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder>? configureBuilder = null)
    {
        IHttpClientBuilder builder = services.AddHttpClient("CartsAPI", configureClient);

        configureBuilder?.Invoke(builder);

        services.AddHttpClient<ICartsClient>("CartsAPI")
            .AddTypedClient<ICartsClient>((http, sp) => new CartsAPI.CartsClient(http));

        return services;
    }
}