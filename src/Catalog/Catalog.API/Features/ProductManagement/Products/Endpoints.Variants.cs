using Catalog.API.Features.ProductManagement.Products.Variants;
using Catalog.API.Model;

using FluentValidation;

using MediatR;

using Microsoft.AspNetCore.Http.HttpResults;

using Asp.Versioning.Builder;
using Catalog.API.Domain.Entities;
using Catalog.API.Features.ProductManagement.Attributes;

namespace Catalog.API.Features.ProductManagement.Products;

public static partial class Endpoints
{
    public static IEndpointRouteBuilder MapProductVariantsEndpoints(this IEndpointRouteBuilder app)
    {
        string GetProductVariantsExpire20 = nameof(GetProductVariantsExpire20);

        var versionedApi = app.NewVersionedApi("Products");

        var group = versionedApi.MapGroup("/v{version:apiVersion}/products")
            .WithTags("Products")
            .HasApiVersion(1, 0)
            .WithOpenApi();

        group.MapGet("{idOrHandle}/variants", GetVariants)
            .WithName($"Products_{nameof(GetVariants)}")
            .CacheOutput(GetProductVariantsExpire20);

        group.MapGet("{idOrHandle}/variants/{variantIdOrHandle}", GetVariant)
            .WithName($"Products_{nameof(GetVariant)}");

        group.MapDelete("{id}/variants{variantId}", DeleteVariant)
            .WithName($"Products_{nameof(DeleteVariant)}");

        group.MapPost("{idOrHandle}/variants/findVariant", FindVariantByAttributeValues)
            .WithName($"Products_{nameof(FindVariantByAttributeValues)}");

        group.MapPost("{idOrHandle}/variants/find2", FindsVariantsByAttributeValues)
            .WithName($"Products_{nameof(FindsVariantsByAttributeValues)}");

        group.MapPost("{idOrHandle}/attributes/{attributeId}/availableValuesForVariant", GetAvailableVariantAttributeValues)
            .WithName($"Products_{nameof(GetAvailableVariantAttributeValues)}");

        group.MapPost("{id}/variants", CreateVariant)
            .WithName($"Products_{nameof(CreateVariant)}");

        group.MapPut("{id}/variants/{variantId}", UpdateVariant)
            .WithName($"Products_{nameof(UpdateVariant)}");

        group.MapPost("{id}/variants/{variantId}/uploadImage", UploadVariantImage)
            .WithName($"Products_{nameof(UploadVariantImage)}");

        return app;
    }

    public static async Task<Results<Ok<PagedResult<ProductDto>>, BadRequest>> GetVariants(string idOrHandle, int page = 1, int pageSize = 10, string? searchString = null, string? sortBy = null, SortDirection? sortDirection = null, IMediator mediator = default, CancellationToken cancellationToken = default)
    {
        return TypedResults.Ok(await mediator.Send(new GetProductVariants(idOrHandle, page, pageSize, searchString, sortBy, sortDirection)));
    }

    public static async Task<Results<Ok, BadRequest>> DeleteVariant(long id, long variantId, IMediator mediator, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteProductVariant(id, variantId), cancellationToken);
        return TypedResults.Ok();
    }

    public static async Task<Results<Ok<ProductDto>, BadRequest>> GetVariant(string idOrHandle, string variantIdOrHandle, IMediator mediator, CancellationToken cancellationToken)
    {
        return TypedResults.Ok(await mediator.Send(new GetProductVariant(idOrHandle, variantIdOrHandle), cancellationToken));
    }

    public static async Task<Results<Ok<ProductDto?>, BadRequest>> FindVariantByAttributeValues(string idOrHandle, Dictionary<string, string?> selectedAttributeValues, IMediator mediator, CancellationToken cancellationToken)
    {
        return TypedResults.Ok(await mediator.Send(new FindProductVariant(idOrHandle, selectedAttributeValues), cancellationToken));
    }

    public static async Task<Results<Ok<IEnumerable<ProductDto>>, BadRequest>> FindsVariantsByAttributeValues(string idOrHandle, Dictionary<string, string?> selectedAttributeValues, IMediator mediator, CancellationToken cancellationToken)
    {
        return TypedResults.Ok(await mediator.Send(new FindProductVariants(idOrHandle, selectedAttributeValues), cancellationToken));
    }

    public static async Task<Results<Ok<IEnumerable<AttributeValueDto>>, BadRequest>> GetAvailableVariantAttributeValues(string idOrHandle, string attributeId, Dictionary<string, string?> selectedAttributeValues, IMediator mediator, CancellationToken cancellationToken)
    {
        return TypedResults.Ok(await mediator.Send(new GetAvailableAttributeValues(idOrHandle, attributeId, selectedAttributeValues), cancellationToken));
    }

    public static async Task<Results<Ok<ProductDto>, ProblemHttpResult>> CreateVariant(long id, CreateProductVariantData data, IMediator mediator, CancellationToken cancellationToken)
    {
        try
        {
            return TypedResults.Ok(await mediator.Send(new CreateProductVariant(id, data), cancellationToken));
        }
        catch (VariantAlreadyExistsException e)
        {
            return TypedResults.Problem(
                title: "Variant already exists.",
                detail: "There is already a variant with the chosen options.",
                //instance: Request.Path,
                statusCode: StatusCodes.Status400BadRequest);
        }
    }

    public static async Task<Results<Ok<ProductDto>, ProblemHttpResult>> UpdateVariant(long id, long variantId, UpdateProductVariantData data, IMediator mediator, CancellationToken cancellationToken)
    {
        try
        {
            return TypedResults.Ok(await mediator.Send(new UpdateProductVariant(id, variantId, data), cancellationToken));
        }
        catch (VariantAlreadyExistsException e)
        {
            return TypedResults.Problem(
                title: "Variant already exists.",
                detail: "There is already a variant with the chosen options.",
                //instance: Request.Path,
                statusCode: StatusCodes.Status400BadRequest);
        }
    }

    public static async Task<Results<Ok<string>, BadRequest>> UploadVariantImage(long id, long variantId, IFormFile file, IMediator mediator, CancellationToken cancellationToken)
    {
        var url = await mediator.Send(new UploadProductVariantImage(id, variantId, file.Name, file.OpenReadStream()), cancellationToken);
        return TypedResults.Ok(url);
    }
}