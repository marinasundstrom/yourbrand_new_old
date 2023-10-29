
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Catalog.API.Features.ProductManagement.Products.Attributes;
using Catalog.API.Model;

using Asp.Versioning.Builder;

namespace Catalog.API.Features.ProductManagement.Products.Attributes;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapProductAttributesEndpoints(this IEndpointRouteBuilder app)
    {
        var versionedApi = app.NewVersionedApi("Products");

        var group = versionedApi.MapGroup("/v{version:apiVersion}/products/{productId}/attributes")
            .WithTags("Products")
            .HasApiVersion(1, 0)
            .WithOpenApi();

        group.MapGet("/", GetProductAttributes)
            .WithName($"Products_{nameof(GetProductAttributes)}");

        group.MapPost("/", AddProductAttribute)
            .WithName($"Products_{nameof(AddProductAttribute)}");

        group.MapPut("/{attributeId}", UpdateProductAttribute)
            .WithName($"Products_{nameof(UpdateProductAttribute)}");

        group.MapDelete("/{attributeId}", DeleteProductAttribute)
            .WithName($"Products_{nameof(DeleteProductAttribute)}");

        return app;
    }

    public static async Task<IEnumerable<ProductAttributeDto>> GetProductAttributes(long productId, IMediator mediator)
    {
        return await mediator.Send(new GetProductAttributes(productId));
    }

    public static async Task<ProductAttributeDto> AddProductAttribute(long productId, AddProductAttributeDto data, IMediator mediator, CancellationToken cancellationToken)
    {
        return await mediator.Send(new AddProductAttribute(productId, data.AttributeId, data.ValueId), cancellationToken);
    }

    public static async Task<ProductAttributeDto> UpdateProductAttribute(long productId, string attributeId, UpdateProductAttributeDto data, IMediator mediator, CancellationToken cancellationToken)
    {
        return await mediator.Send(new UpdateProductAttribute(productId, attributeId, data.ValueId), cancellationToken);
    }

    public static async Task DeleteProductAttribute(long productId, string attributeId, IMediator mediator)
    {
        await mediator.Send(new DeleteProductAttribute(productId, attributeId));
    }

    /*
    [HttpPost("{productId}/Attributes/{attributeId}/GetAvailableValues")]
    public async Task<ActionResult<IEnumerable<Features.Attributes.AttributeValueDto>>> GetAvailableAttributeValues(long productId, string attributeId, Dictionary<string, string?> selectedAttributes)
    {
        return Ok(await _mediator.Send(new GetAvailableAttributeValues(productId, attributeId, selectedAttributes)));
    }
    */
}

public sealed record AddProductAttributeDto(string AttributeId, string ValueId);

public sealed record UpdateProductAttributeDto(string ValueId);