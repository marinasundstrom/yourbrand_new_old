using Blazored.LocalStorage;

using Microsoft.Extensions.DependencyInjection;

namespace YourBrand.Admin.NavMenu;

public static class ServicesExtensions
{
    public static IServiceCollection AddNavigationServices(this IServiceCollection services)
    {
        services.AddScoped<NavManager>();
        return services;
    }
}