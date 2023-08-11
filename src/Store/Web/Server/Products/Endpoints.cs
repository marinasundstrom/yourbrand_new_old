using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Products;

public static class Endpoints 
{
    public static IEndpointRouteBuilder MapProductsEndpoints(this IEndpointRouteBuilder app) 
    {  
        string GetProductsExpire20 = nameof(GetProductsExpire20);

        app.MapGet("/api/products", GetProducts)
            .WithName($"Products_{nameof(GetProducts)}")
            .WithTags("Products")
            .CacheOutput(GetProductsExpire20)
            .RequireCors()
            .WithOpenApi();

        app.MapGet("/api/products/{id}", GetProductById)
            .WithName($"Products_{nameof(GetProductById)}")
            .WithTags("Products")
            .CacheOutput(GetProductsExpire20)
            .RequireCors()
            .WithOpenApi();

        return app;
    }

    private static async Task<Results<Ok<PagedResult<Product>>, NotFound>> GetProducts(int? page = 1, int? pageSize = 10, string? searchTerm = null, string? categoryPath = null, IProductsService productsService = default!, CancellationToken cancellationToken = default)
    {
        var results = await productsService.GetProducts(page, pageSize, searchTerm, categoryPath, cancellationToken);
        return results is not null ? TypedResults.Ok(results) : TypedResults.NotFound();
    }

    private static async Task<Results<Ok<Product>, NotFound>> GetProductById(string id, IProductsService productsService = default!,CancellationToken cancellationToken = default)
    {
        var product = await productsService.GetProductById(id, cancellationToken);
        return product is not null ? TypedResults.Ok(product) : TypedResults.NotFound();
    }
}