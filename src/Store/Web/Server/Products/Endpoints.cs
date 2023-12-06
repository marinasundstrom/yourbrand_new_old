using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Products;

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
            .WithName($"Products_{nameof(GetProducts)}")
            .CacheOutput(OutputCachePolicyNames.GetProductsExpire20);

        productsGroup.MapGet("/{productIdOrHandle}", GetProductById)
            .WithName($"Products_{nameof(GetProductById)}");

        productsGroup.MapPost("/{productIdOrHandle}/findVariant", FindProductVariantByAttributes)
            .WithName($"Products_{nameof(FindProductVariantByAttributes)}");

        productsGroup.MapPost("/{productIdOrHandle}/find", FindProductVariantsByAttributes)
            .WithName($"Products_{nameof(FindProductVariantsByAttributes)}");

        productsGroup.MapGet("/{productIdOrHandle}/variants", GetProductVariants)
            .WithName($"Products_{nameof(GetProductVariants)}");

        productsGroup.MapPost("/{productIdOrHandle}/attributes/{attributeId}/availableValuesForVariant", GetAvailableVariantAttributeValues)
            .WithName($"Products_{nameof(GetAvailableVariantAttributeValues)}");

        return app;
    }

    private static async Task<Results<Ok<PagedResult<Product>>, NotFound>> GetProducts(int? page = 1, int? pageSize = 10, string? searchTerm = null, string? categoryPath = null, IProductsService productsService = default!, CancellationToken cancellationToken = default)
    {
        var results = await productsService.GetProducts(page, pageSize, searchTerm, categoryPath, cancellationToken);
        return results is not null ? TypedResults.Ok(results) : TypedResults.NotFound();
    }

    private static async Task<Results<Ok<Product>, NotFound>> GetProductById(string productIdOrHandle, IProductsService productsService = default!, CancellationToken cancellationToken = default)
    {
        var product = await productsService.GetProductById(productIdOrHandle, cancellationToken);
        return product is not null ? TypedResults.Ok(product) : TypedResults.NotFound();
    }

    private static async Task<Results<Ok<Product>, NotFound>> FindProductVariantByAttributes(string productIdOrHandle, Dictionary<string, string> selectedAttributeValues, IProductsService productsService = default!, CancellationToken cancellationToken = default)
    {
        var product = await productsService.FindProductVariantByAttributes(productIdOrHandle, selectedAttributeValues, cancellationToken);
        return product is not null ? TypedResults.Ok(product) : TypedResults.NotFound();
    }

    private static async Task<Results<Ok<IEnumerable<Product>>, NotFound>> FindProductVariantsByAttributes(string productIdOrHandle, Dictionary<string, string> selectedAttributeValues, IProductsService productsService = default!, CancellationToken cancellationToken = default)
    {
        var products = await productsService.FindProductVariantsByAttributes(productIdOrHandle, selectedAttributeValues, cancellationToken);
        return products is not null ? TypedResults.Ok(products) : TypedResults.NotFound();
    }

    private static async Task<Results<Ok<PagedResult<Product>>, NotFound>> GetProductVariants(string productIdOrHandle, int page = 1, int pageSize = 10, string? searchTerm = null, IProductsService productsService = default!, CancellationToken cancellationToken = default)
    {
        var results = await productsService.GetProductVariants(productIdOrHandle, page, pageSize, searchTerm, cancellationToken);
        return results is not null ? TypedResults.Ok(
                new PagedResult<Product>(results.Items, results.Total)
        ) : TypedResults.NotFound();
    }

    public static async Task<Results<Ok<IEnumerable<AttributeValue>>, BadRequest>> GetAvailableVariantAttributeValues(string productIdOrHandle, string attributeId, Dictionary<string, string?> selectedAttributeValues, IProductsService productsService = default!, CancellationToken cancellationToken = default!)
    {
        var results = await productsService.GetAvailableProductVariantAttributesValues(productIdOrHandle, attributeId, selectedAttributeValues, cancellationToken);
        return results is not null ? TypedResults.Ok(results) : TypedResults.BadRequest();
    }
}