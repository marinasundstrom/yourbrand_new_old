﻿using HealthChecks.UI.Client;

using Microsoft.EntityFrameworkCore;

using YourBrand.YourService.API;
using YourBrand.YourService.API.Features;
using YourBrand.YourService.API.Infrastructure;
using YourBrand.YourService.API.Persistence;

using Steeltoe.Discovery.Client;

using YourBrand;
using YourBrand.Extensions;

using Serilog;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

string ServiceName = builder.Configuration["ServiceName"]!;
string ServiceVersion = "1.0";

// Add services to container

builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(builder.Configuration)
                        .Enrich.WithProperty("Application", ServiceName)
                        .Enrich.WithProperty("Environment", ctx.HostingEnvironment.EnvironmentName));

builder.Services
    .AddOpenApi(ServiceName, ApiVersions.All)
    .AddApiVersioningServices();

builder.Services.AddObservability(ServiceName, ServiceVersion, builder.Configuration);

builder.Services.AddProblemDetails();

builder.Services.AddOutputCache(options =>
{
    options.AddGetProductsPolicy();
});

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDiscoveryClient();
}

if (builder.Environment.IsProduction())
{
    builder.Configuration.AddAzureAppConfiguration(builder.Configuration);

    builder.Configuration.AddAzureKeyVault(builder.Configuration);
}

builder.Services.AddServiceBus(bus =>
{
    // bus.AddConsumersFromNamespaceContaining<>();

    if (builder.Environment.IsDevelopment())
    {
        bus.UsingRabbitMQ(builder.Configuration);
    }
    else
    {
        bus.UsingAzureServiceBus(builder.Configuration);
    }
});

builder.Services.AddSignalR();

builder.Services
    //.AddPresentation()
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddPersistence(builder.Configuration);

builder.Services.AddTenantService();
builder.Services.AddCurrentUserService();

builder.Services.AddAuthenticationServices(builder.Configuration);

builder.Services.AddAuthorization();

builder.Services.AddHealthChecksServices<ApplicationDbContext>();

//builder.Services.AddTransient<ExceptionHandlingMiddleware>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSerilogRequestLogging();

app.MapObservability();

app.UseExceptionHandler();
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseOpenApi();
}

app.UseOutputCache();

//app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapFeaturesEndpoints();

app.MapHealthChecks();

try
{
    using (var scope = app.Services.CreateScope())
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        //await context.Database.MigrateAsync();

        //await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

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

static async Task SeedData(ApplicationDbContext context, IConfiguration configuration, ILogger<Program> logger)
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