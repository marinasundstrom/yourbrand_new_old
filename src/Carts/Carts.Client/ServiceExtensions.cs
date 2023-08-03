﻿namespace Carts;

using Microsoft.Extensions.DependencyInjection;
using CartsAPI;

public static class ServiceExtensions 
{
    public static IServiceCollection AddCartsClient(this IServiceCollection services, string url) 
    {
        services.AddHttpClient("CartsAPI", (sp, http) =>
        {
            http.BaseAddress = new Uri(url);
        });

        services.AddHttpClient<ICartsClient>("CartsAPI")
            .AddTypedClient<ICartsClient>((http, sp) => new CartsAPI.CartsClient(http));
        
        return services;
    }
}