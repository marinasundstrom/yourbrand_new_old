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

        productsGroup.MapGet("/{productIdOrHandle}", GetProductById)
            .WithName($"Products_{nameof(GetProductById)}");

        productsGroup.MapPost("/{productIdOrHandle}/findProductVariants", FindProductVariantByAttributes)
            .WithName($"Products_{nameof(FindProductVariantByAttributes)}");

        productsGroup.MapPost("/{productIdOrHandle}/findProductVariants2", FindProductVariantByAttributes2)
            .WithName($"Products_{nameof(FindProductVariantByAttributes2)}");

        productsGroup.MapGet("/{productIdOrHandle}/variants", GetProductVariants)
            .WithName($"Products_{nameof(GetProductVariants)}");

        return app;
    }

    private static async Task<Results<Ok<PagedResult<Product>>, NotFound>> GetProducts(int? page = 1, int? pageSize = 10, string? searchTerm = null, string? categoryPath = null, IProductsClient productsClient = default!, CancellationToken cancellationToken = default)
    {
        var results = await productsClient.GetProductsAsync(null, null, false, true, page, pageSize, searchTerm, categoryPath, null, null, cancellationToken);
        return results is not null ? TypedResults.Ok(
                new PagedResult<Product>(results.Items.Select(x => x.Map()), results.Total)
        ) : TypedResults.NotFound();
    }

    private static async Task<Results<Ok<Product>, NotFound>> GetProductById(string productIdOrHandle, IProductsClient productsClient = default!, CancellationToken cancellationToken = default)
    {
        var product = await productsClient.GetProductByIdAsync(productIdOrHandle, cancellationToken);
        return product is not null ? TypedResults.Ok(product.Map()) : TypedResults.NotFound();
    }

    private static async Task<Results<Ok<IEnumerable<Product>>, NotFound>> FindProductVariantByAttributes2(string productIdOrHandle, Dictionary<string, string> selectedAttributeValues, IProductsClient productsClient = default!, CancellationToken cancellationToken = default)
    {
        var products = await productsClient.FindVariantByAttributeValues2Async(productIdOrHandle, selectedAttributeValues, cancellationToken);
        return products is not null ? TypedResults.Ok(products.Select(x => x.Map())) : TypedResults.NotFound();
    }

    private static async Task<Results<Ok<IEnumerable<Product>>, NotFound>> FindProductVariantByAttributes(string productIdOrHandle, Dictionary<string, string> selectedAttributeValues, IProductsClient productsClient = default!, CancellationToken cancellationToken = default)
    {
        var products = await productsClient.FindVariantByAttributeValues2Async(productIdOrHandle, selectedAttributeValues, cancellationToken);
        return products is not null ? TypedResults.Ok(products.Select(x => x.Map())) : TypedResults.NotFound();
    }

    private static async Task<Results<Ok<PagedResult<Product>>, NotFound>> GetProductVariants(string productIdOrHandle, int page = 10, int pageSize = 10, string? searchTerm = null, IProductsClient productsClient = default!, CancellationToken cancellationToken = default)
    {
        var results = await productsClient.GetVariantsAsync(productIdOrHandle, page, pageSize, searchTerm, null, null, cancellationToken);
        return results is not null ? TypedResults.Ok(
                new PagedResult<Product>(results.Items.Select(x => x.Map()), results.Total)
        ) : TypedResults.NotFound();
    }
}

public sealed record PagedResult<T>(IEnumerable<T> Items, int Total);