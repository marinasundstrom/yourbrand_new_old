using Microsoft.Extensions.DependencyInjection;

using YourBrand.Admin.AppBar;
using YourBrand.Admin.Markdown;
using YourBrand.Admin.NavMenu;
using YourBrand.Admin.Services;
using YourBrand.Admin.Theming;
using YourBrand.Admin.Widgets;

namespace YourBrand.Admin;

public static class ServiceExtensions
{
    public static IServiceCollection AddShellServices(this IServiceCollection services)
    {
        services
            .AddWidgets()
            .AddThemeServices()
            .AddNavigationServices()
            .AddAppBar()
            .AddTransient<CustomAuthorizationMessageHandler>()
            .AddScoped<ICurrentUserService, CurrentUserService>()
            .AddScoped<IAccessTokenProvider, AccessTokenProvider>()
            .AddMarkdownServices();

        return services;
    }
}