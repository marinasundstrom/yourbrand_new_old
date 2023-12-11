using Blazored.LocalStorage;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Localization;

using MudBlazor;
using MudBlazor.Services;

using YourBrand.Admin;
using YourBrand.Admin.AppBar;
using YourBrand.Admin.Localization;
using YourBrand.Admin.NavMenu;
using YourBrand.Admin.Services;
using YourBrand.Catalog;
using YourBrand.Client;
using YourBrand.Admin.Sales;

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

bool isDebug = false;
#if DEBUG
isDebug = true;
#endif

if (isDebug)
{
    builder.Services.AddOidcAuthentication(options =>
    {
        builder.Configuration.Bind("Local", options.ProviderOptions);
    });
}
else
{
    builder.Services.AddMsalAuthentication(options =>
    {
        builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);

        options.ProviderOptions.LoginMode = "redirect";
    });
}

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddLocalization();

builder.Services.AddShellServices();

builder.Services.AddScoped<IStoreProvider, StoreProvider>();

var app = builder.Build();

app.Services.UseShell();

await app.Services.ApplyLocalization();

YourBrand.Admin.Sales.ServiceExtensions.InitNavBar(app.Services);
YourBrand.Admin.Sales.ServiceExtensions.InitAppBarTray(app.Services);

await app.RunAsync();
