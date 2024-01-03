using Azure.Identity;

using HealthChecks.UI.Client;

using MassTransit;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;

using YourBrand.YourService.API;
using YourBrand.YourService.API.Features;
using YourBrand.YourService.API.Features.OrderManagement;
using YourBrand.YourService.API.Features.OrderManagement.Orders;
using YourBrand.YourService.API.Infrastructure;
using YourBrand.YourService.API.Persistence;

using Steeltoe.Discovery.Client;

using YourBrand;
using YourBrand.Extensions;

var builder = WebApplication.CreateBuilder(args);

string ServiceName = builder.Configuration["ServiceName"];

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDiscoveryClient();
}

string GetCartsExpire20 = nameof(GetCartsExpire20);

builder.Services.AddOutputCache(options =>
{
    options.AddGetProductsPolicy();
});

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

// Add services to the container.

builder.Services
    .AddOpenApi(ServiceName)
    .AddApiVersioningServices();

builder.Services.AddObservability("Sales.API", "1.0", builder.Configuration);

/*
builder.Services.AddSqlServer<AppDbContext>(
    builder.Configuration.GetValue<string>("yourbrand:carts-svc:db:connectionstring"),
    c => c.EnableRetryOnFailure());*/

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

builder.Services.AddSignalR();

builder.Services
    .AddPresentation()
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddPersistence(builder.Configuration);

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddAuthenticationServices(builder.Configuration);

builder.Services.AddAuthorization();

builder.Services
    .AddHealthChecks()
    .AddDbContextCheck<AppDbContext>();

var app = builder.Build();

app.MapObservability();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
}

app.UseOutputCache();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapFeaturesEndpoints();

app.MapHubsForApp();

app.MapHealthChecks("/healthz", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

try
{
    using (var scope = app.Services.CreateScope())
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        //await context.Database.MigrateAsync();

        //await context.Database.EnsureDeletedAsync();
        //await context.Database.EnsureCreatedAsync();

        if (args.Contains("--seed"))
        {
            await SeedData(context, configuration, logger);
            return;
        }
    }
}
catch (Exception e)
{
    Console.WriteLine(e);
}

await app.RunAsync();

static async Task SeedData(AppDbContext context, IConfiguration configuration, ILogger<Program> logger)
{
    try
    {
        await Seed.SeedData(context, configuration);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred seeding the " +
            "database. Error: {Message}", ex.Message);
    }
}

// INFO: Makes Program class visible to IntegrationTests.
public partial class Program { }