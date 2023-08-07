using Azure.Identity;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.ResponseCompression;
using Catalog;
using YourBrand.Server.Products;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddOpenApiDocument(config => {
    config.PostProcess = document =>
    {
        document.Info.Title = "Admin API";
    };

    config.DefaultReferenceTypeNullHandling = NJsonSchema.Generation.ReferenceTypeNullHandling.NotNull;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi(p => p.Path = "/swagger/{documentName}/swagger.yaml");
    app.UseSwaggerUi3(p => p.DocumentPath = "/swagger/{documentName}/swagger.yaml");

    app.UseWebAssemblyDebugging();;
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.MapProductsEndpoints();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();