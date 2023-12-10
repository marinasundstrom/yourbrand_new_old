using Blazored.LocalStorage;

using Microsoft.Extensions.DependencyInjection;

namespace YourBrand.Admin.AppBar;

public static class ServiceExtensions
{
    public static IServiceCollection AddAppBar(this IServiceCollection services)
    {
        services.AddScoped<IAppBarTrayService, AppBarTrayService>();

        return services;
    }
}