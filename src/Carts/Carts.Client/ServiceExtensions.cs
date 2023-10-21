namespace Carts;

using CartsAPI;

using Microsoft.Extensions.DependencyInjection;

using Steeltoe.Common.Http.Discovery;

public static class ServiceExtensions
{
    public static IServiceCollection AddCartsClient(this IServiceCollection services, string url)
    {
        services.AddHttpClient<ICartsClient>("CartsAPI")
            .AddTypedClient<ICartsClient>((http, sp) => new CartsAPI.CartsClient(http));

        return services;
    }
}