using System.Net;

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

        productsGroup.MapPut("/{id}/price", UpdateProductPrice)
            .WithName($"Products_{nameof(UpdateProductPrice)}");

        productsGroup.MapDelete("/{id}", DeleteProduct)
            .WithName($"Products_{nameof(DeleteProduct)}");

        productsGroup.MapPost("/{id}/image", UploadProductImage)
            .WithName($"Products_{nameof(UploadProductImage)}")
            .DisableAntiforgery();

        productsGroup.MapPut("/{id}/handle", UpdateProductHandle)
            .WithName($"Products_{nameof(UpdateProductHandle)}");

        productsGroup.MapPut("/{id}/visibility", UpdateProductVisibility)
            .WithName($"Products_{nameof(UpdateProductVisibility)}");

        productsGroup.MapPut("/{id}/category", UpdateProductCategory)
            .WithName($"Products_{nameof(UpdateProductCategory)}");

        productsGroup.MapGet("/{id}/variants", GetProductVariants)
            .WithName($"Products_{nameof(GetProductVariants)}");

        productsGroup.MapPost("/{id}/variants", CreateProductVariant)
            .WithName($"Products_{nameof(CreateProductVariant)}")
            .ProducesProblem(StatusCodes.Status400BadRequest);

        productsGroup.MapDelete("/{id}/variants/{variantId}", DeleteProductVariant)
            .WithName($"Products_{nameof(DeleteProductVariant)}");

        productsGroup.MapGet("/{id}/attributes", GetProductAttributes)
            .WithName($"Products_{nameof(GetProductAttributes)}");

        productsGroup.MapPost("/{id}/attributes", AddProductAttribute)
            .WithName($"Products_{nameof(AddProductAttribute)}");

        productsGroup.MapPut("/{id}/attributes/{attributeId}", UpdateProductAttribute)
            .WithName($"Products_{nameof(UpdateProductAttribute)}");

        productsGroup.MapDelete("/{id}/attributes/{attributeId}", DeleteProductAttribute)
            .WithName($"Products_{nameof(DeleteProductAttribute)}");

        productsGroup.MapDelete("/import", ImportProducts)
            .WithName($"Products_{nameof(ImportProducts)}")
            .Produces<ProductImportResult>(StatusCodes.Status200OK)
            .DisableAntiforgery();

        return app;
    }

    private static async Task<Results<Ok<ProductImportResult>, BadRequest>> ImportProducts(IFormFile file, CatalogAPI.IProductsClient productsClient = default!, CancellationToken cancellationToken = default!)
    {
        var result = await productsClient.ImportProductsAsync(
            new FileParameter(file.OpenReadStream(), file.FileName, file.ContentType), cancellationToken);

        return TypedResults.Ok(result);
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

    private static async Task<Results<Ok<Product>, ProblemHttpResult>> CreateProductVariant(long id, CreateProductVariantData request, CatalogAPI.IProductsClient productsClient, CancellationToken cancellationToken)
    {
        try
        {
            var product = await productsClient.CreateVariantAsync(id, request, cancellationToken);
            return TypedResults.Ok(product);
        }
        catch (ApiException<CatalogAPI.ProblemDetails> exc)
        {
            Console.WriteLine("Foo" + exc);

            var details = exc.Result;

            return TypedResults.Problem(details.Detail, details.Instance, details.Status, details.Title, details.Type);
        }
    }

    private static async Task<Results<Ok, NotFound>> DeleteProductVariant(long id, long variantId, CatalogAPI.IProductsClient productsClient, CancellationToken cancellationToken)
    {
        await productsClient.DeleteVariantAsync(id, variantId, cancellationToken);
        return TypedResults.Ok();
    }

    private static async Task<Results<Ok<ICollection<ProductAttribute>>, BadRequest>> GetProductAttributes(long id, CatalogAPI.IProductsClient productsClient, CancellationToken cancellationToken = default!)
    {
        return TypedResults.Ok(await productsClient.GetProductAttributesAsync(id, cancellationToken));
    }

    private static async Task<Results<Ok<ProductAttribute>, BadRequest>> AddProductAttribute(long id, AddProductAttribute request, CatalogAPI.IProductsClient productsClient, CancellationToken cancellationToken)
    {
        var productAttribute = await productsClient.AddProductAttributeAsync(id, request, cancellationToken);
        return TypedResults.Ok(productAttribute);
    }

    private static async Task<Results<Ok, NotFound>> UpdateProductAttribute(long id, string attributeId, UpdateProductAttribute request, CatalogAPI.IProductsClient productsClient, CancellationToken cancellationToken)
    {
        await productsClient.UpdateProductAttributeAsync(id, attributeId, request, cancellationToken);
        return TypedResults.Ok();
    }


    private static async Task<Results<Ok, NotFound>> DeleteProductAttribute(long id, string attributeId, CatalogAPI.IProductsClient productsClient, CancellationToken cancellationToken)
    {
        await productsClient.DeleteProductAttributeAsync(id, attributeId, cancellationToken);
        return TypedResults.Ok();
    }
}