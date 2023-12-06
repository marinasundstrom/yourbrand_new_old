using BlazorApp;

using BlazorApp;
using BlazorApp.Cart;
using BlazorApp.ProductCategories;
using BlazorApp.Products;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;

using StoreFront;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services
    .AddTransient<CookieHandler>();

builder.Services
    .AddHttpClient("WebAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<CookieHandler>()
    .AddStandardResilienceHandler();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
    .CreateClient("WebAPI"));

builder.Services.AddScoped<RenderingContext, ClientRenderingContext>();

var baseUri = new Uri(builder.HostEnvironment.BaseAddress + "storefront/");

var catalogApiHttpClient = builder.Services.AddStoreFrontClients(baseUri,
clientBuilder =>
{
    //clientBuilder.AddStandardResilienceHandler();
});

builder.Services
    .AddProductsServices()
    .AddProductCategoriesServices()
    .AddCartServices();

await builder.Build().RunAsync();