using Blazored.LocalStorage;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Localization;

using MudBlazor;
using MudBlazor.Services;

using YourBrand.Admin;
using YourBrand.Admin.AppBar;
using YourBrand.Admin.Localization;
using YourBrand.Admin.NavMenu;
using YourBrand.Catalog;
using YourBrand.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services
    .AddHttpClient("WebAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
    .CreateClient("WebAPI"));

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;

    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = true;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 5000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

var baseUri = new Uri(builder.HostEnvironment.BaseAddress + "catalog/");

var catalogApiHttpClient = builder.Services.AddCatalogClients(baseUri,
clientBuilder =>
{
    //clientBuilder.AddStandardResilienceHandler();
});

builder.Services.AddApiAuthorization();

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddLocalization();

builder.Services.AddShellServices();

builder.Services.AddScoped<IStoreProvider, StoreProvider>();

var app = builder.Build();

app.Services.UseShell();

await app.Services.ApplyLocalization();

InitNavBar(app.Services);
InitAppBarTray(app.Services);

await app.RunAsync();

static void InitNavBar(IServiceProvider services)
{
    var navManager = services
       .GetRequiredService<NavManager>();

    var t = services.GetRequiredService<IStringLocalizer<Resources>>();

    var group = navManager.GetGroup("sales") ?? navManager.CreateGroup("sales", () => t["Sales"]);
    group.RequiresAuthorization = false; //true;

    var catalogItem = group.CreateGroup("catalog", options =>
    {
        options.Name = t["Catalog"];
        options.Icon = MudBlazor.Icons.Material.Filled.Book;
    });

    catalogItem.CreateItem("products", () => t["Products"], MudBlazor.Icons.Material.Filled.FormatListBulleted, "/products");

    catalogItem.CreateItem("categories", () => t["Categories"], MudBlazor.Icons.Material.Filled.Collections, "/products/categories");

    catalogItem.CreateItem("attributes", () => t["Attributes"], MudBlazor.Icons.Material.Filled.List, "/products/attributes");

    catalogItem.CreateItem("brands", () => t["Brands"], MudBlazor.Icons.Material.Filled.List, "/brands");

    catalogItem.CreateItem("stores", () => t["Stores"], MudBlazor.Icons.Material.Filled.Store, "/stores");
}

static void InitAppBarTray(IServiceProvider services)
{
    var appBarTray = services
        .GetRequiredService<IAppBarTrayService>();

    var snackbar = services
        .GetRequiredService<ISnackbar>();

    var t = services.GetRequiredService<IStringLocalizer<Resources>>();

    appBarTray.AddItem(new AppBarTrayItem("show", () => t["Store"], typeof(StoreSelector)));
}