using Blazored.LocalStorage;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

using MudBlazor;

using YourBrand.Admin.AppBar;
using YourBrand.Admin.NavMenu;
using YourBrand.Admin.Services;
using YourBrand.Catalog;

using Store = YourBrand.Admin.Services.Store;

namespace YourBrand.Admin.Sales.OrderManagement;

public static class ServiceExtensions
{
    public static IServiceProvider UseOrderManagement(this IServiceProvider services)
    {
        services.InitNavBar();

        return services;
    }

    private static void InitNavBar(this IServiceProvider services)
    {
        var navManager = services
           .GetRequiredService<NavManager>();

        var t = services.GetRequiredService<IStringLocalizer<YourBrand.Admin.Sales.Catalog.Resources>>();

        var group = navManager.GetGroup("sales") ?? navManager.CreateGroup("sales", () => t["Sales"]);
        group.RequiresAuthorization = true;

        group.CreateItem("orders", () => t["Orders"], MudBlazor.Icons.Material.Filled.InsertDriveFile, "/orders");
    }
}