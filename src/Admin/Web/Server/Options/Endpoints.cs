using CatalogAPI;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;

namespace YourBrand.Server.Options;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapOptionsEndpoints(this IEndpointRouteBuilder app)
    {
        string GetProductsExpire20 = nameof(GetProductsExpire20);

        var versionedApi = app.NewVersionedApi("ProductCategories");

        var productsGroup = versionedApi.MapGroup("/api/v{version:apiVersion}/options")
            .WithTags("Options")
            .HasApiVersion(1, 0)
            .WithOpenApi();

        productsGroup.MapGet("/", GetOptions)
            .WithName($"Options_{nameof(GetOptions)}");

        productsGroup.MapGet("{id}", GetOptionValues)
            .WithName($"Options_{nameof(GetOptionValues)}");

        return app;
    }

    private static async Task<Ok<ICollection<Option>>> GetOptions(string id, CatalogAPI.IOptionsClient optionsClient = default!, CancellationToken cancellationToken = default!)
    {
        return TypedResults.Ok(await optionsClient.GetOptionsAsync(id, cancellationToken));
    }

    private static async Task<Ok<ICollection<CatalogAPI.OptionValue>>> GetOptionValues(string id, CatalogAPI.IOptionsClient optionsClient = default!, CancellationToken cancellationToken = default!)
    {
        return TypedResults.Ok(await optionsClient.GetOptionValuesAsync(id, cancellationToken));
    }

    /*

    private static async Task<Results<Ok<CatalogAPI.Option>, BadRequest, ProblemHttpResult>> CreateOption(CreateOption request, CatalogAPI.IOptionsClient optionsClient = default!, CancellationToken cancellationToken = default!)
    {
        return TypedResults.Ok(await optionsClient.add(request, cancellationToken));
    }

    private static async Task<Results<Ok, NotFound>> UpdateOption(string id, UpdateOption request, CatalogAPI.IOptionsClient optionsClient = default!, CancellationToken cancellationToken = default!)
    {
        await optionsClient.UpdateOptionAsync(id, request, cancellationToken);
        return TypedResults.Ok();
    }

    private static async Task<Results<Ok, NotFound>> DeleteOption(string id, CatalogAPI.IOptionsClient optionsClient = default!, CancellationToken cancellationToken = default!)
    {
        await optionsClient.DeleteOptionAsync(id, cancellationToken);
        return TypedResults.Ok();
    }
    */
}