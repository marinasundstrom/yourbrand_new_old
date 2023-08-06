using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using YourBrand.Client;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services
    .AddHttpClient("WebAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
    .CreateClient("WebAPI"));

builder.Services.AddMudServices();

builder.Services.AddHttpClient<AdminAPI.IProductsClient>("WebAPI")
        .AddTypedClient<AdminAPI.IProductsClient>((http, sp) => new AdminAPI.ProductsClient(http));

await builder.Build().RunAsync();