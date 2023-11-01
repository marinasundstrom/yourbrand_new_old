using System.Security.Claims;
using System.Threading.RateLimiting;

using Azure.Identity;

using BlazorApp;
using BlazorApp.Cart;
using BlazorApp.Data;
using YourBrand.Extensions;
using BlazorApp.ProductCategories;
using BlazorApp.Products;

using StoreFront;

using HealthChecks.UI.Client;

using MassTransit;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Http.Resilience;

using Serilog;

using Steeltoe.Common.Http.Discovery;
using Steeltoe.Discovery.Client;

using YourBrand;

string MyAllowSpecificOrigins = nameof(MyAllowSpecificOrigins);

string serviceName = "Store.Web";
string serviceVersion = "1.0";

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDiscoveryClient();
}

builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(builder.Configuration)
                        .Enrich.WithProperty("Application", serviceName)
                        .Enrich.WithProperty("Environment", ctx.HostingEnvironment.EnvironmentName));

builder.Services.AddRateLimiterForIPAddress(builder.Configuration);

builder.Services.AddOutputCache(options =>
{
    options.AddPolicy(OutputCachePolicyNames.GetProductsExpire20, builder =>
    {
        builder.Expire(TimeSpan.FromSeconds(20));
        builder.SetVaryByQuery("page", "pageSize", "searchTerm");
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins(
                    "https://yourbrand-store-web.kindgrass-70ab37e8.swedencentral.azurecontainerapps.io",
                    "https://localhost:7188",
                    "https://localhost:8080")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

AddClients(builder);

builder.Services
    .AddProductsServices()
    .AddProductCategoriesServices()
    .AddCartServices();

if (builder.Environment.IsProduction())
{
    builder.Configuration.AddAzureAppConfiguration(options =>
        options.Connect(
            new Uri($"https://{builder.Configuration["Azure:AppConfig:Name"]}.azconfig.io"),
            new DefaultAzureCredential()));

    builder.Configuration.AddAzureKeyVault(
        new Uri($"https://{builder.Configuration["Azure:KeyVault:Name"]}.vault.azure.net/"),
        new DefaultAzureCredential());
}

builder.Services
    .AddOpenApi(serviceName)
    .AddApiVersioningServices();

builder.Services.AddObservability(serviceName, serviceVersion, builder.Configuration);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAuthorization()
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<ApplicationDbContext>(c => c.UseInMemoryDatabase("db"));

builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();

builder.Services.AddSingleton<RenderingContext, ServerRenderingContext>();

builder.Services.AddSingleton<RequestContext>();

builder.Services.AddLocalization();

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    x.AddConsumers(typeof(Program).Assembly);

    if (builder.Environment.IsProduction())
    {
        x.UsingAzureServiceBus((context, cfg) =>
        {
            cfg.Host($"sb://{builder.Configuration["Azure:ServiceBus:Namespace"]}.servicebus.windows.net");

            cfg.ConfigureEndpoints(context);
        });
    }
    else
    {
        x.UsingRabbitMq((context, cfg) =>
        {
            var rabbitmqHost = builder.Configuration["RABBITMQ_HOST"] ?? "localhost";

            cfg.Host(rabbitmqHost, "/", h =>
            {
                h.Username("guest");
                h.Password("guest");
            });

            cfg.ConfigureEndpoints(context);
        });
    }
});

builder.Services
    .AddHealthChecks();

var app = builder.Build();

app.UseSerilogRequestLogging();

app.MapObservability();

app.UseStatusCodePagesWithRedirects("/error/{0}");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseOpenApi();

//app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors(MyAllowSpecificOrigins);

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddAdditionalAssemblies(typeof(BlazorApp.CookieHandler).Assembly)
    .AddInteractiveWebAssemblyRenderMode()
    .AddInteractiveServerRenderMode();

app
    .MapProductsEndpoints()
    .MapProductCategoriesEndpoints()
    .MapCartEndpoints();

app.MapGroup("/identity").MapIdentityApi<IdentityUser>();

app.MapGet("/requires-auth", (ClaimsPrincipal user) => $"Hello, {user.Identity?.Name}!").RequireAuthorization();

app.MapGet("/api/weatherforecast", async (DateOnly startDate, IWeatherForecastService weatherForecastService, CancellationToken cancellationToken) =>
{
    var forecasts = await weatherForecastService.GetWeatherForecasts(startDate, cancellationToken);
    return Results.Ok(forecasts);
})
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.MapHealthChecks("/healthz", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();

static void AddClients(WebApplicationBuilder builder)
{
    var storefrontHttpClient = builder.Services.AddStoreFrontClients(new Uri(builder.Configuration["yourbrand:storefront-svc:url"]!),
    clientBuilder =>
    {
        clientBuilder.AddStandardResilienceHandler();

        if (builder.Environment.IsDevelopment())
        {
            clientBuilder.AddServiceDiscovery();
        }
    });
}
