using CatalogAPI;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;

namespace YourBrand.Server.Attributes;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapAttributesEndpoints(this IEndpointRouteBuilder app)
    {
        string GetProductsExpire20 = nameof(GetProductsExpire20);

        var versionedApi = app.NewVersionedApi("Attributes");

        var productsGroup = versionedApi.MapGroup("/api/v{version:apiVersion}/attributes")
            .WithTags("Attributes")
            .HasApiVersion(1, 0)
            .WithOpenApi();

        productsGroup.MapGet("/", GetAttributes)
            .WithName($"Attributes_{nameof(GetAttributes)}");

        productsGroup.MapGet("{id}", GetAttributeById)
            .WithName($"Attributes_{nameof(GetAttributeById)}");

        productsGroup.MapPost("/", CreateAttribute)
            .WithName($"Attributes_{nameof(CreateAttribute)}");

        productsGroup.MapPut("{id}", UpdateAttribute)
            .WithName($"Attributes_{nameof(UpdateAttribute)}");

        productsGroup.MapDelete("{id}", DeleteAttribute)
            .WithName($"Attributes_{nameof(DeleteAttribute)}");

        return app;
    }

    private static async Task<Ok<PagedResultOfAttribute>> GetAttributes(IEnumerable<string> ids, int page = 1, int pageSize = 10, string? searchTerm = null, string? sortBy = null, SortDirection? sortDirection = null, CatalogAPI.IAttributesClient attributesClient = default!, CancellationToken cancellationToken = default!)
    {
        return TypedResults.Ok(await attributesClient.GetAttributesAsync(ids, page, pageSize, searchTerm, sortBy, sortDirection, cancellationToken));
    }

    private static async Task<Ok<CatalogAPI.Attribute>> GetAttributeById(string id, CatalogAPI.IAttributesClient attributesClient = default!, CancellationToken cancellationToken = default!)
    {
        return TypedResults.Ok(await attributesClient.GetAttributeByIdAsync(id, cancellationToken));
    }

    private static async Task<Results<Ok<CatalogAPI.Attribute>, BadRequest, ProblemHttpResult>> CreateAttribute(CreateAttribute request, CatalogAPI.IAttributesClient attributesClient = default!, CancellationToken cancellationToken = default!)
    {
        return TypedResults.Ok(await attributesClient.CreateAttributeAsync(request, cancellationToken));
    }

    private static async Task<Results<Ok, NotFound>> UpdateAttribute(string id, UpdateAttribute request, CatalogAPI.IAttributesClient attributesClient = default!, CancellationToken cancellationToken = default!)
    {
        await attributesClient.UpdateAttributeAsync(id, request, cancellationToken);
        return TypedResults.Ok();
    }

    private static async Task<Results<Ok, NotFound>> DeleteAttribute(string id, CatalogAPI.IAttributesClient attributesClient = default!, CancellationToken cancellationToken = default!)
    {
        await attributesClient.DeleteAttributeAsync(id, cancellationToken);
        return TypedResults.Ok();
    }
}