using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using MudBlazor;
using MudBlazor.Services;

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

builder.Services.AddHttpClient<AdminAPI.IProductsClient>("WebAPI")
        .AddTypedClient<AdminAPI.IProductsClient>((http, sp) => new AdminAPI.ProductsClient(http));

builder.Services.AddHttpClient<AdminAPI.IProductCategoriesClient>("WebAPI")
        .AddTypedClient<AdminAPI.IProductCategoriesClient>((http, sp) => new AdminAPI.ProductCategoriesClient(http));

builder.Services.AddHttpClient<AdminAPI.IAttributesClient>("WebAPI")
        .AddTypedClient<AdminAPI.IAttributesClient>((http, sp) => new AdminAPI.AttributesClient(http));

builder.Services.AddHttpClient<AdminAPI.IOptionsClient>("WebAPI")
        .AddTypedClient<AdminAPI.IOptionsClient>((http, sp) => new AdminAPI.OptionsClient(http));

await builder.Build().RunAsync();