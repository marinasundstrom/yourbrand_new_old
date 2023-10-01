﻿using Azure.Storage.Blobs;
using Azure.Identity;
using YourBrand;
using Catalog.API.Products;
using Catalog.API.ProductCategories;
using Catalog.API.Data;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using MassTransit;
using FluentValidation;
using Catalog.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

string GetProductsExpire20 = nameof(GetProductsExpire20);

builder.Services.AddOutputCache(options =>
{
    options.AddPolicy(GetProductsExpire20, builder => 
    {
        builder.Expire(TimeSpan.FromSeconds(20));
        builder.SetVaryByQuery("page", "pageSize", "searchTerm", "categoryPath", "sortBy", "sortDirection");
    });
});

if(builder.Environment.IsProduction()) 
{
    builder.Configuration.AddAzureKeyVault(
        new Uri($"https://{builder.Configuration["KeyVaultName"]}.vault.azure.net/"),
        new DefaultAzureCredential());
}

builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));

builder.Services.AddAzureClients(clientBuilder =>
{
    // Add a KeyVault client
    clientBuilder.AddSecretClient(new Uri($"https://{builder.Configuration["KeyVaultName"]}.vault.azure.net/"));

    // Add a Storage account client
    if(builder.Environment.IsDevelopment()) 
    {
        clientBuilder.AddBlobServiceClient(builder.Configuration["yourbrand-storage-connectionstring"])
                        .WithVersion(BlobClientOptions.ServiceVersion.V2019_07_07);
    }
    else 
    {
        clientBuilder.AddBlobServiceClient(new Uri($"https://{builder.Configuration["StorageName"]}.blob.core.windows.net"));
    }

    // Use DefaultAzureCredential by default
    clientBuilder.UseCredential(new DefaultAzureCredential());
});

// Add services to the container.

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddObservability("Catalog.API", "1.0", builder.Configuration);

builder.Services.AddSqlServer<CatalogContext>(
    builder.Configuration.GetValue<string>("yourbrand-catalog-db-connectionstring")
    ?? builder.Configuration.GetConnectionString("CatalogDb"),
    c => c.EnableRetryOnFailure());

builder.Services.AddScoped<WeatherForecastService>();

builder.Services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<Program>());

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
    .AddHealthChecks()
    .AddDbContextCheck<CatalogContext>();

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

app
    .MapProductsEndpoints()
    .MapProductCategoriesEndpoints();

app.MapHealthChecks("/healthz", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

/*
app.MapGet("/api/weatherforecast", async (DateOnly startDate, WeatherForecastService weatherForecastService, CancellationToken cancellationToken) =>
    {
        var forecasts = await weatherForecastService.GetWeatherForecasts(startDate, cancellationToken);
        return Results.Ok(forecasts);
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();
*/

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
        await Seed.SeedData(context, configuration);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred seeding the " +
            "database. Error: {Message}", ex.Message);
    }
}