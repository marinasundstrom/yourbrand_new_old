using BlazorApp;
using Client;
using Client.Cart;
using Client.Products;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services
    .AddTransient<CookieHandler>();

builder.Services
    .AddHttpClient("WebAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<CookieHandler>()
    .AddPolicyHandler(GetRetryPolicy());

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
    .CreateClient("WebAPI"));

builder.Services.AddScoped<RenderingContext, ClientRenderingContext>();

builder.Services.AddHttpClient<IWeatherForecastService, ClientWeatherForecastService>("WebAPI");

builder.Services
    .AddProductsServices()
    .AddCartServices();

await builder.Build().RunAsync();

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
        .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                                                                    retryAttempt)));
}