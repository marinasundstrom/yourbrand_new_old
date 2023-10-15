using Catalog.API.Features.ProductManagement.Products;
using Catalog.API.Features.ProductManagement.Products.Variants;
using Catalog.API.Model;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Features.ProductManagement;

partial class ProductsController : Controller
{
    [HttpGet("{idOrHandle}/variants")]
    public async Task<ActionResult<PagedResult<ProductDto>>> GetVariants(string idOrHandle, int page = 0, int pageSize = 10, string? searchString = null, string? sortBy = null, SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new GetProductVariants(idOrHandle, page, pageSize, searchString, sortBy, sortDirection)));
    }

    [HttpDelete("{id}/variants/{variantId}")]
    public async Task<ActionResult> DeleteVariant(long id, long variantId)
    {
        await _mediator.Send(new DeleteProductVariant(id, variantId));
        return Ok();
    }

    [HttpGet("{idOrHandle}/variants/{variantIdOrHandle}")]
    public async Task<ActionResult<ProductDto>> GetVariant(string idOrHandle, string variantIdOrHandle)
    {
        return Ok(await _mediator.Send(new GetProductVariant(idOrHandle, variantIdOrHandle)));
    }

    [HttpPost("{idOrHandle}/variants/find")]
    public async Task<ActionResult<ProductDto>> FindVariantByAttributeValues(string idOrHandle, Dictionary<string, string?> selectedAttributeValues)
    {
        return Ok(await _mediator.Send(new FindProductVariant(idOrHandle, selectedAttributeValues)));
    }

    [HttpPost("{idOrHandle}/variants/find2")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> FindVariantByAttributeValues2(string idOrHandle, Dictionary<string, string?> selectedAttributeValues)
    {
        return Ok(await _mediator.Send(new FindProductVariants(idOrHandle, selectedAttributeValues)));
    }

    [HttpGet("{id}/Variants/{variantId}/options")]
    public async Task<ActionResult<ProductVariantAttributeDto>> GetVariantAttributes(long id, long variantId)
    {
        return Ok(await _mediator.Send(new GetProductVariantAttributes(id, variantId)));
    }

    [HttpPost("{id}/variants")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductDto>> CreateVariant(long id, CreateProductVariantData data)
    {
        try
        {
            return Ok(await _mediator.Send(new CreateProductVariant(id, data)));
        }
        catch (VariantAlreadyExistsException e)
        {
            return Problem(
                title: "Variant already exists.",
                detail: "There is already a variant with the chosen options.",
                instance: Request.Path,
                statusCode: StatusCodes.Status400BadRequest);
        }
    }

    [HttpPut("{id}/variants/{variantId}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductDto>> UpdateVariant(long id, long variantId, UpdateProductVariantData data)
    {
        try
        {
            return Ok(await _mediator.Send(new UpdateProductVariant(id, variantId, data)));
        }
        catch (VariantAlreadyExistsException e)
        {
            return Problem(
                title: "Variant already exists.",
                detail: "There is already a variant with the chosen options.",
                instance: Request.Path,
                statusCode: StatusCodes.Status400BadRequest);
        }
    }

    [HttpPost("{id}/variants/{variantId}/uploadImage")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<ActionResult> UploadVariantImage([FromRoute] long id, long variantId, IFormFile file, CancellationToken cancellationToken)
    {
        var url = await _mediator.Send(new UploadProductVariantImage(id, variantId, file.Name, file.OpenReadStream()), cancellationToken);
        return Ok(url);
    }
}