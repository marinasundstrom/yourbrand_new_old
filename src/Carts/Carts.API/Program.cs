using Azure.Identity;
using YourBrand;
using Carts.API;
using Carts.API.Data;
using Carts.API.Extensions;
using MassTransit;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string GetCartsExpire20 = nameof(GetCartsExpire20);

builder.Services.AddOutputCache(options =>
{
    options.AddPolicy(GetCartsExpire20, builder => 
    {
        builder.Expire(TimeSpan.FromSeconds(20));
        builder.SetVaryByQuery("page", "pageSize", "searchTerm");
    });
});

if (builder.Environment.IsProduction())
{
    builder.Configuration.AddAzureKeyVault(
        new Uri($"https://{builder.Configuration["KeyVaultName"]}.vault.azure.net/"),
        new DefaultAzureCredential());
}

// Add services to the container.

builder.Services.AddOpenApi();

builder.Services.AddObservability("Carts.API", "1.0", builder.Configuration);

builder.Services.AddSqlServer<CartsContext>(
    builder.Configuration.GetValue<string>("yourbrand-carts-db-connectionstring")
    ?? builder.Configuration.GetConnectionString("CartsDb"),
    c => c.EnableRetryOnFailure());

builder.Services.AddScoped<WeatherForecastService>();

builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblyContaining<Program>());

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
    .AddDbContextCheck<CartsContext>();

var app = builder.Build();

app.MapObservability();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
}

app.UseOutputCache();

app.UseHttpsRedirection();

app.MapCartsEndpoints();

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
        var context = scope.ServiceProvider.GetRequiredService<CartsContext>();
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
catch(Exception e) 
{
    Console.WriteLine(e);
}

await app.RunAsync();

static async Task SeedData(CartsContext context, IConfiguration configuration, ILogger<Program> logger)
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