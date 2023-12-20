using Blazored.LocalStorage;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

using MudBlazor;

using YourBrand.Admin.AppBar;
using YourBrand.Admin.NavMenu;
using YourBrand.Admin.Sales.Catalog;
using YourBrand.Admin.Sales.OrderManagement;
using YourBrand.Admin.Services;
using YourBrand.Catalog;

using Store = YourBrand.Admin.Services.Store;

namespace YourBrand.Admin.Sales;

public static class ServiceExtensions
{
    public static IServiceProvider UseSales(this IServiceProvider services)
    {
        services.InitNavBar();

        services.UseOrderManagement();
        services.UseCatalog();

        return services;
    }

    private static void InitNavBar(this IServiceProvider services)
    {
        var navManager = services
           .GetRequiredService<NavManager>();

        var t = services.GetRequiredService<IStringLocalizer<YourBrand.Admin.Sales.Resources>>();

        var group = navManager.GetGroup("sales") ?? navManager.CreateGroup("sales", () => t["Sales"]);
        group.RequiresAuthorization = true;
    }
}