using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

using YourBrand.Admin.AppBar;
using YourBrand.Admin.Authentication;
using YourBrand.Admin.Localization;
using YourBrand.Admin.NavMenu;
using YourBrand.Admin.Theming;
using YourBrand.Admin.Widgets;

namespace YourBrand.Admin;

public static class ServiceProviderExtensions
{
    public static IServiceProvider UseShell(this IServiceProvider services)
    {
        CreateNavMenu(services);

        CreateDashboard(services);

        return services
            .UseTheming()
            .UseLocalization()
            .UseAuthentication();
    }

    private static void CreateNavMenu(IServiceProvider services)
    {
        var navManager = services
            .GetRequiredService<NavManager>();

        var t = services.GetRequiredService<IStringLocalizer<YourBrand.Admin.Resources>>();

        navManager.CreateItem("home", options =>
        {
            options.NameFunc = () => t["Home"];
            options.Icon = MudBlazor.Icons.Material.Filled.Home;
            options.Href = "/";
            options.RequiresAuthorization = false;
            options.Index = 0;
        });

        /*
        var group = navManager.GetGroup("administration") ?? navManager.CreateGroup("administration", () => t["Administration"]);

        group.CreateItem("users", options =>
        {
            options.NameFunc = () => t["Users"];
            options.Icon = MudBlazor.Icons.Material.Filled.Person;
            options.Href = "/users";
            options.RequiresAuthorization = true;
        });

        group.CreateItem("setup", options =>
        {
            options.NameFunc = () => t["SetUp"];
            options.Icon = MudBlazor.Icons.Material.Filled.Settings;
            options.Href = "/setup";
        });
        */
    }

    private static void CreateDashboard(IServiceProvider services)
    {
        var widgetService =
            services.GetRequiredService<IWidgetService>();

        widgetService.RegisterWidget(new Widget("sample-widget", "Sample Widget", typeof(SampleWidget)));

        widgetService.RegisterWidget(new Widget("sample-widget2", "Sample Widget", typeof(SampleWidget2))
        {
            Size = WidgetSize.Small
        });

        /*
        widgetService.RegisterWidget(new Widget("sample-widget3", "Sample Widget", typeof(SampleWidget2)) {
            Size = WidgetSize.Large
        });
        */
    }
}