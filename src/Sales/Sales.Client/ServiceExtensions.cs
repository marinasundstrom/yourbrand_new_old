﻿namespace YourBrand.Sales;

using Microsoft.Extensions.DependencyInjection;

public static class ServiceExtensions
{
    public static IServiceCollection AddSalesClients(this IServiceCollection services, Uri baseUrl, Action<IHttpClientBuilder>? configureBuilder = null)
    {
        services.AddOrdersClient((sp, http) =>
        {
            http.BaseAddress = baseUrl;
        }, configureBuilder);

        services.AddOrderStatusesClient((sp, http) =>
        {
            http.BaseAddress = baseUrl;
        }, configureBuilder);

        return services;
    }

    public static IServiceCollection AddOrdersClient(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder>? configureBuilder = null)
    {
        IHttpClientBuilder builder = services.AddHttpClient("SalesAPI", configureClient);

        configureBuilder?.Invoke(builder);

        services.AddHttpClient<IOrdersClient>("SalesAPI")
            .AddTypedClient<IOrdersClient>((http, sp) => new YourBrand.Sales.OrdersClient(http));

        return services;
    }

    public static IServiceCollection AddOrderStatusesClient(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder>? configureBuilder = null)
    {
        IHttpClientBuilder builder = services.AddHttpClient("SalesAPI", configureClient);

        configureBuilder?.Invoke(builder);

        services.AddHttpClient<IOrderStatusesClient>("SalesAPI")
            .AddTypedClient<IOrderStatusesClient>((http, sp) => new YourBrand.Sales.OrderStatusesClient(http));

        return services;
    }
}