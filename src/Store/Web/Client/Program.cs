using BlazorApp;

using Client;
using Client.Cart;
using Client.ProductCategories;
using Client.Products;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;

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

builder.Services.AddHttpClient<IWeatherForecastService, ClientWeatherForecastService>("WebAPI");

builder.Services
    .AddProductsServices()
    .AddProductCategoriesServices()
    .AddCartServices();

await builder.Build().RunAsync();