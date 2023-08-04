using BlazorApp;
using Client;
using Client.Cart;
using Client.Products;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services
    .AddTransient<CookieHandler>();

builder.Services
    .AddHttpClient("WebAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<CookieHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
    .CreateClient("WebAPI"));

builder.Services.AddScoped<RenderingContext, ClientRenderingContext>();

builder.Services.AddHttpClient<IWeatherForecastService, ClientWeatherForecastService>("WebAPI");

builder.Services
    .AddProductsServices()
    .AddCartServices();

await builder.Build().RunAsync();