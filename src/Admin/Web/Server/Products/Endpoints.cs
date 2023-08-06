using CatalogAPI;
using Microsoft.AspNetCore.Http.HttpResults;

namespace YourBrand.Server.Products;

public static class Endpoints 
{
    public static IEndpointRouteBuilder MapProductsEndpoints(this IEndpointRouteBuilder app) 
    {  
        string GetProductsExpire20 = nameof(GetProductsExpire20);

        var productsGroup = app.MapGroup("/api/products");

        productsGroup.MapGet("/", GetProducts)
            .WithName($"Products_{nameof(GetProducts)}")
            .WithTags("Products")
            .WithOpenApi();
            //.CacheOutput(GetProductsExpire20);

        productsGroup.MapGet("/{id}", GetProductById)
            .WithName($"Products_{nameof(GetProductById)}")
            .WithTags("Products")
            .WithOpenApi();

        productsGroup.MapPost("/", CreateProduct)
            .WithName($"Products_{nameof(CreateProduct)}")
            .WithTags("Products")
            .WithOpenApi();

        productsGroup.MapPut("/{id}", UpdateProductDetails)
            .WithName($"Products_{nameof(UpdateProductDetails)}")
            .WithTags("Products")
            .WithOpenApi();

        productsGroup.MapPut("/price", UpdateProductPrice)
            .WithName($"Products_{nameof(UpdateProductPrice)}")
            .WithTags("Products")
            .WithOpenApi();

        productsGroup.MapDelete("/{id}", DeleteProduct)
            .WithName($"Products_{nameof(DeleteProduct)}")
            .WithTags("Products")
            .WithOpenApi();

        return app;
    }

    private static async Task<Ok<PagedResultOfProduct>> GetProducts(int page = 1, int pageSize = 10, string? searchTerm = null, CatalogAPI.IProductsClient productsClient = default!, CancellationToken cancellationToken = default!)
    {
        return TypedResults.Ok(await productsClient.GetProductsAsync(page, pageSize, searchTerm, cancellationToken));
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
}