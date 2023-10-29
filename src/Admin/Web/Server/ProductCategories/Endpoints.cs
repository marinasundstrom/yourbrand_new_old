using CatalogAPI;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;

namespace YourBrand.Server.ProductCategories;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapProductCategoriesEndpoints(this IEndpointRouteBuilder app)
    {
        string GetProductsExpire20 = nameof(GetProductsExpire20);

        var versionedApi = app.NewVersionedApi("ProductCategories");

        var productsGroup = versionedApi.MapGroup("/api/v{version:apiVersion}/productCategories")
            .WithTags("ProductCategories")
            .HasApiVersion(1, 0)
            .WithOpenApi();

        productsGroup.MapGet("{*path}", GetProductsCategories)
            .WithName($"ProductCategories_{nameof(GetProductsCategories)}");

        productsGroup.MapGet("{idOrPath}", GetProductCategoryById)
            .WithName($"ProductCategories_{nameof(GetProductCategoryById)}");

        productsGroup.MapPost("/", CreateProductCategory)
            .WithName($"ProductCategories_{nameof(CreateProductCategory)}");

        productsGroup.MapPut("{idOrPath}", UpdateProductCategoryDetails)
            .WithName($"ProductCategories_{nameof(UpdateProductCategoryDetails)}");

        productsGroup.MapDelete("{idOrPath}", DeleteProductCategory)
            .WithName($"ProductCategories_{nameof(DeleteProductCategory)}");

        return app;
    }

    private static async Task<Ok<PagedResultOfProductCategory>> GetProductsCategories(int page = 1, int pageSize = 10, string? searchTerm = null, CatalogAPI.IProductCategoriesClient productCategoriesClient = default!, CancellationToken cancellationToken = default!)
    {
        return TypedResults.Ok(await productCategoriesClient.GetProductCategoriesAsync(page, pageSize, searchTerm, cancellationToken));
    }

    private static async Task<Ok<CatalogAPI.ProductCategory>> GetProductCategoryById(string idOrPath, CatalogAPI.IProductCategoriesClient productCategoriesClient = default!, CancellationToken cancellationToken = default!)
    {
        return TypedResults.Ok(await productCategoriesClient.GetProductCategoryByIdAsync(idOrPath, cancellationToken));
    }

    private static async Task<Results<Ok<CatalogAPI.ProductCategory>, BadRequest, ProblemHttpResult>> CreateProductCategory(CreateProductCategoryRequest request, CatalogAPI.IProductCategoriesClient productCategoriesClient = default!, CancellationToken cancellationToken = default!)
    {
        return TypedResults.Ok(await productCategoriesClient.CreateProductCategoryAsync(request, cancellationToken));
    }

    private static async Task<Results<Ok, NotFound>> UpdateProductCategoryDetails(string idOrPath, UpdateProductCategoryDetailsRequest request, CatalogAPI.IProductCategoriesClient productCategoriesClient = default!, CancellationToken cancellationToken = default!)
    {
        await productCategoriesClient.UpdateProductCategoryDetailsAsync(idOrPath, request, cancellationToken);
        return TypedResults.Ok();
    }

    private static async Task<Results<Ok, NotFound>> DeleteProductCategory(string idOrPath, CatalogAPI.IProductCategoriesClient productCategoriesClient = default!, CancellationToken cancellationToken = default!)
    {
        await productCategoriesClient.DeleteProductCategoryAsync(idOrPath, cancellationToken);
        return TypedResults.Ok();
    }
}