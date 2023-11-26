using CatalogAPI;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;

namespace YourBrand.Server.Products;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapProductsEndpoints(this IEndpointRouteBuilder app)
    {
        string GetProductsExpire20 = nameof(GetProductsExpire20);

        var versionedApi = app.NewVersionedApi("Products");

        var productsGroup = versionedApi.MapGroup("/api/v{version:apiVersion}/products")
            .WithTags("Products")
            .HasApiVersion(1, 0)
            .WithOpenApi();

        productsGroup.MapGet("/", GetProducts)
            .WithName($"Products_{nameof(GetProducts)}");

        productsGroup.MapGet("/{id}", GetProductById)
            .WithName($"Products_{nameof(GetProductById)}");

        productsGroup.MapPost("/", CreateProduct)
            .WithName($"Products_{nameof(CreateProduct)}");

        productsGroup.MapPut("/{id}", UpdateProductDetails)
            .WithName($"Products_{nameof(UpdateProductDetails)}");

        productsGroup.MapPut("/price", UpdateProductPrice)
            .WithName($"Products_{nameof(UpdateProductPrice)}");

        productsGroup.MapDelete("/{id}", DeleteProduct)
            .WithName($"Products_{nameof(DeleteProduct)}");

        productsGroup.MapPost("/{id}/image", UploadProductImage)
            .WithName($"Products_{nameof(UploadProductImage)}")
            .DisableAntiforgery();

        productsGroup.MapPut("/handle", UpdateProductHandle)
            .WithName($"Products_{nameof(UpdateProductHandle)}");

        productsGroup.MapPut("/visibility", UpdateProductVisibility)
            .WithName($"Products_{nameof(UpdateProductVisibility)}");

        productsGroup.MapPut("/category", UpdateProductCategory)
            .WithName($"Products_{nameof(UpdateProductCategory)}");

        productsGroup.MapGet("/{id}/variants", GetProductVariants)
            .WithName($"Products_{nameof(GetProductVariants)}");

        return app;
    }

    private static async Task<Ok<PagedResultOfProduct>> GetProducts(int page = 1, int pageSize = 10, string? searchTerm = null, string? sortBy = null, SortDirection? sortDirection = null, CatalogAPI.IProductsClient productsClient = default!, CancellationToken cancellationToken = default!)
    {
        return TypedResults.Ok(await productsClient.GetProductsAsync(null, null, true, true, page, pageSize, searchTerm, null, sortBy, sortDirection, cancellationToken));
    }

    private static async Task<Results<Ok<Product>, NotFound>> GetProductById(string id, CatalogAPI.IProductsClient productsClient, CancellationToken cancellationToken)
    {
        return TypedResults.Ok(await productsClient.GetProductByIdAsync(id, cancellationToken));
    }

    private static async Task<Results<Ok<Product>, BadRequest>> CreateProduct(CreateProductRequest request, CatalogAPI.IProductsClient productsClient, CancellationToken cancellationToken)
    {
        var product = await productsClient.CreateProductAsync(request, cancellationToken);
        return TypedResults.Ok(product);
    }

    private static async Task<Results<Ok, NotFound>> UpdateProductDetails(string id, UpdateProductDetailsRequest request, CatalogAPI.IProductsClient productsClient, CancellationToken cancellationToken)
    {
        await productsClient.UpdateProductDetailsAsync(id, request, cancellationToken);
        return TypedResults.Ok();
    }

    private static async Task<Results<Ok, NotFound>> UpdateProductPrice(string id, UpdateProductPriceRequest request, CatalogAPI.IProductsClient productsClient, CancellationToken cancellationToken)
    {
        await productsClient.UpdateProductPriceAsync(id, request, cancellationToken);
        return TypedResults.Ok();
    }

    private static async Task<Results<Ok, NotFound>> DeleteProduct(string id, CatalogAPI.IProductsClient productsClient, CancellationToken cancellationToken)
    {
        await productsClient.DeleteProductAsync(id, cancellationToken);
        return TypedResults.Ok();
    }

    private static async Task<Results<Ok<string>, NotFound>> UploadProductImage(string id, IFormFile file,
        CatalogAPI.IProductsClient productsClient, CancellationToken cancellationToken)
    {
        var path = await productsClient.UploadProductImageAsync(id,
            new CatalogAPI.FileParameter(file.OpenReadStream(), file.FileName, file.ContentType), cancellationToken);

        return TypedResults.Ok(path);
    }

    private static async Task<Results<Ok, NotFound>> UpdateProductHandle(string id, UpdateProductHandleRequest request, CatalogAPI.IProductsClient productsClient, CancellationToken cancellationToken)
    {
        await productsClient.UpdateProductHandleAsync(id, request, cancellationToken);
        return TypedResults.Ok();
    }

    private static async Task<Results<Ok, NotFound>> UpdateProductVisibility(string id, UpdateProductVisibilityRequest request, CatalogAPI.IProductsClient productsClient, CancellationToken cancellationToken)
    {
        await productsClient.UpdateProductVisibilityAsync(id, new UpdateProductVisibilityRequest()
        {
            Visibility = request.Visibility
        });
        return TypedResults.Ok();
    }

    private static async Task<Results<Ok, NotFound>> UpdateProductCategory(string id, UpdateProductCategoryRequest request, CatalogAPI.IProductsClient productsClient, CancellationToken cancellationToken)
    {
        await productsClient.UpdateProductCategoryAsync(id, request, cancellationToken);
        return TypedResults.Ok();
    }

    private static async Task<Results<Ok<PagedResultOfProduct>, NotFound>> GetProductVariants(string id, int page = 1, int pageSize = 10, string? searchTerm = null, string? sortBy = null, SortDirection? sortDirection = null, CatalogAPI.IProductsClient productsClient = default!, CancellationToken cancellationToken = default!)
    {
        return TypedResults.Ok(await productsClient.GetVariantsAsync(id, page, pageSize, searchTerm, sortBy, sortDirection, cancellationToken));
    }
}