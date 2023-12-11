using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

using MudBlazor;

using YourBrand.Admin.AppBar;
using YourBrand.Admin.Authentication;

namespace YourBrand.Admin.Authentication;

public static class ServiceProviderExtensions
{
    public static IServiceProvider UseAuthentication(this IServiceProvider services)
    {
        AddAppBarTrayItems(services);

        return services;
    }

    private static void AddAppBarTrayItems(IServiceProvider services)
    {
        var appBarTray = services
            .GetRequiredService<IAppBarTrayService>();

        var t = services.GetRequiredService<IStringLocalizer<AppBar.AppBar>>();

        var loginDisplayId = "Shell.LoginDisplay";

        appBarTray.AddItem(new AppBarTrayItem(loginDisplayId, string.Empty, typeof(LoginDisplay)));
    }
}