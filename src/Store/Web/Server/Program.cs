using System.Security.Claims;
using Azure.Identity;
using YourBrand;
using BlazorApp;
using BlazorApp.Cart;
using BlazorApp.Products;
using BlazorApp.ProductCategories;
using BlazorApp.Data;
using MassTransit;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CartsAPI;
using CatalogAPI;
using System.Threading.RateLimiting;
using BlazorApp.Extensions;
using Serilog;

string MyAllowSpecificOrigins = nameof(MyAllowSpecificOrigins);

string serviceName = "Store.Web";
string serviceVersion = "1.0";

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, cfg) =>  cfg.ReadFrom.Configuration(builder.Configuration)
                        .Enrich.WithProperty("Application", serviceName)
                        .Enrich.WithProperty("Environment", ctx.HostingEnvironment.EnvironmentName));

string GetProductsExpire20 = nameof(GetProductsExpire20);

builder.Services.AddRateLimiter(options => 
{
    options. RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    
    options.AddPolicy("fixed", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(), 
        factory: _ => new FixedWindowRateLimiterOptions 
        {
            PermitLimit = 10,
            Window = TimeSpan.FromSeconds(10)
        }));
});

builder.Services.AddOutputCache(options =>
{
    options.AddPolicy(GetProductsExpire20, builder => 
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

builder.Services.AddHttpClient("CatalogAPI", (sp, http) =>
{
    http.BaseAddress = new Uri(builder.Configuration["yourbrand-catalog-api-url"]!);
});

builder.Services.AddHttpClient<IProductsClient>("CatalogAPI")
.AddTypedClient<IProductsClient>((http, sp) => new CatalogAPI.ProductsClient(http));

builder.Services.AddHttpClient("CatalogAPI", (sp, http) =>
{
    http.BaseAddress = new Uri(builder.Configuration["yourbrand-catalog-api-url"]!);
});

builder.Services.AddHttpClient<IProductCategoriesClient>("CatalogAPI")
.AddTypedClient<IProductCategoriesClient>((http, sp) => new CatalogAPI.ProductCategoriesClient(http));

builder.Services.AddHttpClient("CartsAPI", (sp, http) =>
{
    http.BaseAddress = new Uri(builder.Configuration["yourbrand-carts-api-url"]!);
});

builder.Services.AddHttpClient<ICartsClient>("CartsAPI")
.AddTypedClient<ICartsClient>((http, sp) => new CartsClient(http));

builder.Services
    .AddProductsServices()
    .AddProductCategoriesServices()
    .AddCartServices();

//builder.Services.AddCartsClient(builder.Configuration["yourbrand-carts-api-url"]!);

if (builder.Environment.IsProduction())
{
    builder.Configuration.AddAzureKeyVault(
        new Uri($"https://{builder.Configuration["KeyVaultName"]}.vault.azure.net/"),
        new DefaultAzureCredential());
}

builder.Services.AddOpenApi();

builder.Services.AddObservability(serviceName, serviceVersion, builder.Configuration);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddWebAssemblyComponents()
    .AddServerComponents();

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

builder.Services.AddScoped<ServerNavigationManager>();

builder.Services.AddLocalization();

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    x.AddConsumers(typeof(Program).Assembly);

    if(builder.Environment.IsProduction()) 
    {
        x.UsingAzureServiceBus((context, cfg) => {
            cfg.Host(builder.Configuration["yourbrand-servicebus-connectionstring"]);

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

app.MapRazorComponents<App>()
    .AddWebAssemblyRenderMode()
    .AddServerRenderMode();

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

app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.MapHealthChecks("/healthz", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
