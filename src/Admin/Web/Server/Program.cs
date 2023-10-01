using Azure.Identity;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using YourBrand;
using Catalog;
using YourBrand.Server.Products;
using YourBrand.Server.ProductCategories;
using YourBrand.Server.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

string serviceName = "Admin.Web";
string serviceVersion = "1.0";

builder.Host.UseSerilog((ctx, cfg) =>  cfg.ReadFrom.Configuration(builder.Configuration)
                        .Enrich.WithProperty("Application", serviceName)
                        .Enrich.WithProperty("Environment", ctx.HostingEnvironment.EnvironmentName));

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

// Add services to the container.

if (builder.Environment.IsProduction())
{
    builder.Configuration.AddAzureKeyVault(
        new Uri($"https://{builder.Configuration["KeyVaultName"]}.vault.azure.net/"),
        new DefaultAzureCredential());
}

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddCatalogClients(builder.Configuration["yourbrand-catalog-api-url"]);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddObservability(serviceName, serviceVersion, builder.Configuration);

builder.Services
    .AddHealthChecks();
//    .AddDbContextCheck<ApplicationDbContext>();

var app = builder.Build();

app.UseSerilogRequestLogging();

app.MapObservability();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();

    app.UseWebAssemblyDebugging();;
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app
    .MapProductsEndpoints()
    .MapProductCategoriesEndpoints();

app.MapHealthChecks("/healthz", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.Run();