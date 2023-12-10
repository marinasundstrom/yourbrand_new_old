using Blazored.LocalStorage;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using MudBlazor;
using MudBlazor.Services;

using YourBrand.Admin;
using YourBrand.Admin.Localization;
using YourBrand.Catalog;

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

var app = builder.Build();

app.Services.UseShell();

await app.Services.ApplyLocalization();

await app.RunAsync();
