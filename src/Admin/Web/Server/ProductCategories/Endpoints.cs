using CatalogAPI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Http.HttpResults;

namespace YourBrand.Server.ProductCategories;

public static class Endpoints 
{
    public static IEndpointRouteBuilder MapProductCategoriesEndpoints(this IEndpointRouteBuilder app) 
    {  
        string GetProductsExpire20 = nameof(GetProductsExpire20);

        var productsGroup = app.MapGroup("/api/ProductCategories");

        productsGroup.MapGet("/", GetProductsCategories)
            .WithName($"ProductCategories_{nameof(GetProductsCategories)}")
            .WithTags("ProductCategories")
            .WithOpenApi();
            //.CacheOutput(GetProductsExpire20);

        return app;
    }

    private static async Task<Ok<PagedResultOfProductCategory>> GetProductsCategories(int page = 1, int pageSize = 10, string? searchTerm = null, CatalogAPI.IProductCategoriesClient productCategoriesClient = default!, CancellationToken cancellationToken = default!)
    {
        return TypedResults.Ok(await productCategoriesClient.GetProductCategoriesAsync(page, pageSize, searchTerm, cancellationToken));
    }
}