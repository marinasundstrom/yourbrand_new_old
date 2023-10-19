﻿namespace Carts;

using CartsAPI;

using Microsoft.Extensions.DependencyInjection;

using Steeltoe.Common.Http.Discovery;

public static class ServiceExtensions
{
    public static IServiceCollection AddCartsClient(this IServiceCollection services, string url)
    {
        services.AddHttpClient("CartsAPI", (sp, http) =>
        {
            http.BaseAddress = new Uri(url);
        });

        services.AddHttpClient<ICartsClient>("CartsAPI")
            .AddServiceDiscovery()
            .AddTypedClient<ICartsClient>((http, sp) => new CartsAPI.CartsClient(http));

        return services;
    }
}