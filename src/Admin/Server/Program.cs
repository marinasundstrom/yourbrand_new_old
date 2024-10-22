﻿using Azure.Identity;

using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;

using MudBlazor.Services;

using Serilog;

using Steeltoe.Discovery.Client;

using YourBrand;
using YourBrand.Catalog;
using YourBrand.Extensions;
using YourBrand.Server;
using YourBrand.Server.Extensions;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDiscoveryClient();
}

string serviceName = "Admin.Web";
string serviceVersion = "1.0";

builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(builder.Configuration)
                        .Enrich.WithProperty("Application", serviceName)
                        .Enrich.WithProperty("Environment", ctx.HostingEnvironment.EnvironmentName));

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

// Add services to the container.

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

builder.Services.AddControllers();

builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddInteractiveServerComponents();

AddClients(builder);

builder.Services
    .AddOpenApi(serviceName, ApiVersions.All)
    .AddApiVersioningServices();

builder.Services.AddObservability(serviceName, serviceVersion, builder.Configuration);

builder.Services
    .AddHealthChecksServices();
//    .AddDbContextCheck<ApplicationDbContext>();


builder.Services.AddAuthenticationServices(builder.Configuration);

builder.Services.AddAuthorization();

var reverseProxy = builder.Services.AddReverseProxy();

if (builder.Environment.IsDevelopment())
{
    reverseProxy.LoadFromMemory();
}
else
{
    reverseProxy.LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
}

var app = builder.Build();

app.UseSerilogRequestLogging();

app.MapObservability();

app.MapReverseProxy();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();

    app.UseWebAssemblyDebugging(); ;
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseStaticFiles();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddAdditionalAssemblies(
        typeof(YourBrand.Client.Pages.Counter).Assembly,
        typeof(YourBrand.Admin.Sales.ServiceExtensions).Assembly)
    .AddInteractiveWebAssemblyRenderMode()
    .AddInteractiveServerRenderMode()
    .AllowAnonymous();

app.MapHealthChecks("/healthz", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapControllers();

var foo = app.NewVersionedApi("foo");

foo.MapGet("/v{version:apiVersion}/test", () => "Foo")
    .WithName("Test_test")
    .RequireAuthorization();

app.Run();

static void AddClients(WebApplicationBuilder builder)
{
    var catalogApiHttpClient = builder.Services.AddCatalogClients(new Uri($"{builder.Configuration["yourbrand:admin-web:url"]!}/catalog"),
    clientBuilder =>
    {
        clientBuilder.AddStandardResilienceHandler();

        if (builder.Environment.IsDevelopment())
        {
            //clientBuilder.AddServiceDiscovery();
        }
    });
}