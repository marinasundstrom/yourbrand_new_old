﻿using Azure.Identity;

using Carts;

using Catalog;

using HealthChecks.UI.Client;

using IdentityModel.Client;

using MassTransit;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Identity.Client;

using Steeltoe.Common.Http.Discovery;
using Steeltoe.Discovery.Client;

using StoreFront.API.Features.Cart;
using StoreFront.API.Features.ProductCategories;
using StoreFront.API.Features.Products;
using StoreFront.API.Persistence;

using YourBrand;
using YourBrand.Carts;
using YourBrand.Catalog;
using YourBrand.Extensions;

string ServiceName = "StoreFront.API";

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDiscoveryClient();
}

builder.Services.AddCors();

builder.Services.AddOutputCache(options =>
{
    options.AddGetProductsPolicy();

    options.AddGetProductByIdPolicy();
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

builder.Services.AddObservability("StoreFront.API", "1.0", builder.Configuration);

builder.Services.AddSqlServer<StoreFrontContext>(
    builder.Configuration.GetValue<string>("yourbrand:carts-svc:db:connectionstring"),
    c => c.EnableRetryOnFailure());

builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblyContaining<Program>());

builder.Services.AddCartServices();

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
    .AddDbContextCheck<StoreFrontContext>();

var accessToken = await GetAccessToken(builder.Configuration);

AddClients(builder, accessToken!);

var app = builder.Build();

app.MapObservability();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
}

app.UseOutputCache();

app.UseHttpsRedirection();

app.UseCors();

app.MapCartEndpoints()
    .MapProductCategoriesEndpoints()
    .MapProductsEndpoints();

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
        var context = scope.ServiceProvider.GetRequiredService<StoreFrontContext>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

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

static async Task SeedData(StoreFrontContext context, IConfiguration configuration, ILogger<Program> logger)
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

static void AddClients(WebApplicationBuilder builder, string accessToken)
{
    var catalogApiHttpClient = builder.Services.AddCatalogClients((sp, httpClient) =>
    {
        httpClient.BaseAddress = new Uri(builder.Configuration["yourbrand:catalog-svc:url"]!);
        httpClient.SetBearerToken(accessToken);
    },
    clientBuilder =>
    {
        clientBuilder.AddStandardResilienceHandler();

        if (builder.Environment.IsDevelopment())
        {
            clientBuilder.AddServiceDiscovery();
        }
    });

    var cartsApiHttpClient = builder.Services.AddCartsClient(new Uri(builder.Configuration["yourbrand:carts-svc:url"]!),
    clientBuilder =>
    {
        clientBuilder.AddStandardResilienceHandler();

        if (builder.Environment.IsDevelopment())
        {
            clientBuilder.AddServiceDiscovery();
        }
    });
}

static async Task<string?> GetAccessToken(IConfiguration configuration)
{
    var isDev = configuration["ASPNETCORE_ENVIRONMENT"] == "Development";

    if (isDev)
    {
        return await Local(configuration);
    }

    return await Production(configuration);
}

static async Task<string?> Production(IConfiguration configuration)
{
    IConfidentialClientApplication app = ConfidentialClientApplicationBuilder.Create(configuration.GetValue<string>("AzureAd:ClientId"))
           .WithClientSecret(configuration.GetValue<string>("AzureAd:ClientSecret"))
           .Build();

    var c = app.AcquireTokenForClient(["https://graph.microsoft.com/.default"]);
    var x = await c.ExecuteAsync();
    return x.AccessToken;
}

static async Task<string?> Local(IConfiguration configuration)
{
    // discover endpoints from metadata
    var client = new HttpClient();

    var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5041");
    if (disco.IsError)
    {
        Console.WriteLine(disco.Error);
        throw new Exception();
    }

    // request token
    TokenResponse tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
    {
        Address = disco.TokenEndpoint,

        ClientId = configuration.GetValue<string>("ApiClient:ClientId")!,
        ClientSecret = configuration.GetValue<string>("ApiClient:ClientSecret"),
        Scope = configuration.GetValue<string>("ApiClient:Scope"),
    });
    if (tokenResponse.IsError)
    {
        Console.WriteLine(tokenResponse.Error);
    }

    Console.WriteLine(tokenResponse.Json);

    return tokenResponse.AccessToken;
}

// INFO: Makes Program class visible to IntegrationTests.
public partial class Program { }
