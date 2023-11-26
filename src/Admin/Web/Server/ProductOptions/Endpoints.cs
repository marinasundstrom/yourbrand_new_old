using CatalogAPI;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;

namespace YourBrand.Server.ProductOptions;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapProductOptionsEndpoints(this IEndpointRouteBuilder app)
    {
        var versionedApi = app.NewVersionedApi("Products");

        var productsGroup = versionedApi.MapGroup("/api/v{version:apiVersion}/products/{id}/options")
            .WithTags("Products")
            .HasApiVersion(1, 0)
            .WithOpenApi();

        productsGroup.MapGet("/", GetProductOptions)
            .WithName($"ProductOptions_{nameof(GetProductOptions)}");

        /*
        productsGroup.MapGet("/{id}", GetProductById)
            .WithName($"Products_{nameof(GetProductById)}");
        */

        productsGroup.MapPost("/", CreateProductOption)
            .WithName($"Products_{nameof(CreateProductOption)}");

        productsGroup.MapPut("/{optionId}", UpdateProductOption)
            .WithName($"Products_{nameof(UpdateProductOption)}");

        productsGroup.MapDelete("/{optionId}", DeleteProductOption)
            .WithName($"Products_{nameof(DeleteProductOption)}");

        return app;
    }

    private static async Task<Results<Ok<ICollection<ProductOption>>, BadRequest>> GetProductOptions(long id, string? variantId, CatalogAPI.IProductOptionsClient productOptionsClient = default!, CancellationToken cancellationToken = default!)
    {
        return TypedResults.Ok(await productOptionsClient.GetProductOptionsAsync(id, variantId, cancellationToken));
    }

    /*
    private static async Task<Results<Ok<Product>, NotFound>> GetProductOptionById(string id, CatalogAPI.IProductOptionsClient productOptionsClient, CancellationToken cancellationToken)
    {
        return TypedResults.Ok(await productOptionsClient.Get(id, cancellationToken));
    }
    */

    private static async Task<Results<Ok<Option>, BadRequest>> CreateProductOption(long id, CreateProductOptionData request, CatalogAPI.IProductOptionsClient productOptionsClient, CancellationToken cancellationToken)
    {
        var productOption = await productOptionsClient.CreateProductOptionAsync(id, request, cancellationToken);
        return TypedResults.Ok(productOption);
    }

    private static async Task<Results<Ok, NotFound>> UpdateProductOption(long id, string optionId, UpdateProductOptionData request, CatalogAPI.IProductOptionsClient productOptionsClient, CancellationToken cancellationToken)
    {
        await productOptionsClient.UpdateProductOptionAsync(id, optionId, request, cancellationToken);
        return TypedResults.Ok();
    }


    private static async Task<Results<Ok, NotFound>> DeleteProductOption(long id, string optionId, CatalogAPI.IProductOptionsClient productOptionsClient, CancellationToken cancellationToken)
    {
        await productOptionsClient.DeleteProductOptionAsync(id, optionId, cancellationToken);
        return TypedResults.Ok();
    }
}