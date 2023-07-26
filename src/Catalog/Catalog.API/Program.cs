﻿using Azure.Identity;
using Catalog.API.Products;
using Catalog.API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string GetProductsExpire20 = nameof(GetProductsExpire20);

builder.Services.AddOutputCache(options =>
{
    options.AddPolicy(GetProductsExpire20, builder => 
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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSqlServer<CatalogContext>(
    builder.Configuration.GetValue<string>("yourbrand-catalog-db-connectionstring")
    ?? builder.Configuration.GetConnectionString("CatalogDb"),
    c => c.EnableRetryOnFailure());

builder.Services.AddScoped<WeatherForecastService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseOutputCache();

app.UseHttpsRedirection();

app.MapProductsEndpoints();

/*
app.MapGet("/api/weatherforecast", async (DateOnly startDate, WeatherForecastService weatherForecastService, CancellationToken cancellationToken) =>
    {
        var forecasts = await weatherForecastService.GetWeatherForecasts(startDate, cancellationToken);
        return Results.Ok(forecasts);
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();
*/

try 
{
    using (var scope = app.Services.CreateScope())
    {
        using var context = scope.ServiceProvider.GetRequiredService<CatalogContext>();

        //await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
    }
}
catch(Exception e) 
{
    Console.WriteLine(e);
}

await app.RunAsync();

