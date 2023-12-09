using Azure.Identity;
using Azure.Storage.Blobs;

using Catalog.API.Common;
using YourBrand.Extensions;
using Catalog.API.Features;
using Catalog.API.Features.ProductManagement.Products.Variants;
using Catalog.API.Persistence;

using FluentValidation;

using HealthChecks.UI.Client;

using MassTransit;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;

using Steeltoe.Discovery.Client;

using YourBrand;
using Catalog.API.Features.ProductManagement.Products;

string ServiceName = "Catalog.API";

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDiscoveryClient();
}

builder.Services.AddOutputCache(options =>
{
    options.AddPolicy(OutputCachePolicyNames.GetProductsExpire20, builder =>
    {
        builder.Expire(TimeSpan.FromSeconds(0.6));
        builder.SetVaryByQuery("page", "pageSize", "searchTerm", "categoryPathOrId", "sortBy", "sortDirection");
    });
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

builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));

builder.Services.AddAzureClients(clientBuilder =>
{
    if (builder.Environment.IsProduction())
    {
        // Add a KeyVault client
        clientBuilder.AddSecretClient(new Uri($"https://{builder.Configuration["Azure:KeyVault:Name"]}.vault.azure.net/"));
    }

    // Add a Storage account client
    if (builder.Environment.IsDevelopment())
    {
        clientBuilder.AddBlobServiceClient(builder.Configuration["Azure:StorageAccount:ConnectionString"])
                        .WithVersion(BlobClientOptions.ServiceVersion.V2019_07_07);
    }
    else
    {
        clientBuilder.AddBlobServiceClient(new Uri($"https://{builder.Configuration["Azure:StorageAccount:Name"]}.blob.core.windows.net"));
    }

    // Use DefaultAzureCredential by default
    clientBuilder.UseCredential(new DefaultAzureCredential());
});

// Add services to the container.

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

builder.Services
    .AddOpenApi(ServiceName)
    .AddApiVersioningServices();

builder.Services.AddProductsServices();

builder.Services.AddObservability("Catalog.API", "1.0", builder.Configuration);

builder.Services.AddSqlServer<CatalogContext>(
    builder.Configuration.GetValue<string>("yourbrand:catalog-svc:db:connectionstring"),
    c => c.UseAzureSqlDefaults());

builder.Services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<Program>());

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
    .AddHealthChecks()
    .AddDbContextCheck<CatalogContext>();

builder.Services.AddScoped<ProductVariantsService>();

builder.Services.AddScoped<ICurrentUserService>(sp => null!);

var app = builder.Build();

app.MapObservability();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
}

app.UseOutputCache();

app.UseHttpsRedirection();

app.MapControllers();

app.MapFeaturesEndpoints();

app.MapHealthChecks("/healthz", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var context = scope.ServiceProvider.GetRequiredService<CatalogContext>();
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

    //await context.Database.EnsureDeletedAsync();
    //await context.Database.EnsureCreatedAsync(); 

    if (args.Contains("--seed"))
    {
        await SeedData(context, configuration, logger);
        return;
    }
}

app.Run();

static async Task SeedData(CatalogContext context, IConfiguration configuration, ILogger<Program> logger)
{
    try
    {
        await Seed2.SeedData(context, configuration);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred seeding the " +
            "database. Error: {Message}", ex.Message);
    }
}