using CatalogAPI;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace StoreFront.API.Features.Products;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapProductsEndpoints(this IEndpointRouteBuilder app)
    {
        var versionedApi = app.NewVersionedApi("Products");

        var productsGroup = versionedApi.MapGroup("/api/v{version:apiVersion}/products")
            .WithTags("Products")
            .HasApiVersion(1, 0)
            .WithOpenApi()
            .RequireCors();

        productsGroup.MapGet("/", GetProducts)
            .WithName($"Products_{nameof(GetProducts)}");
        //.CacheOutput(OutputCachePolicyNames.GetProductsExpire20);

        productsGroup.MapGet("/{id}", GetProductById)
            .WithName($"Products_{nameof(GetProductById)}");

        return app;
    }

    private static async Task<Results<Ok<PagedResult<Product>>, NotFound>> GetProducts(int? page = 1, int? pageSize = 10, string? searchTerm = null, string? categoryPath = null, IProductsClient productsClient = default!, CancellationToken cancellationToken = default)
    {
        var results = await productsClient.GetProductsAsync(null, null, false, true, page, pageSize, searchTerm, categoryPath, null, null, cancellationToken);
        return results is not null ? TypedResults.Ok(
                new PagedResult<Product>(results.Items.Select(x => x.Map()), results.Total)
        ) : TypedResults.NotFound();
    }

    private static async Task<Results<Ok<Product>, NotFound>> GetProductById(string id, IProductsClient productsClient = default!, CancellationToken cancellationToken = default)
    {
        var product = await productsClient.GetProductByIdAsync(id, cancellationToken);
        return product is not null ? TypedResults.Ok(product.Map()) : TypedResults.NotFound();
    }
}

public sealed record PagedResult<T>(IEnumerable<T> Items, int Total);