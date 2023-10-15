using Catalog.API.Features.ProductManagement;
using Catalog.API.Features.ProductManagement.Options;
using Catalog.API.Features.ProductManagement.Products;
using Catalog.API.Features.ProductManagement.Products.Options;
using Catalog.API.Features.ProductManagement.Products.Options.Groups;

using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Features.ProductManagement;

partial class ProductsController : Controller
{
    [HttpGet("{id}/options")]
    public async Task<ActionResult<IEnumerable<ProductOptionDto>>> GetProductOptions(long id, string? variantId, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetProductOptions(id, variantId), cancellationToken));
    }

    [HttpPost("{id}/options")]
    public async Task<ActionResult<OptionDto>> CreateProductOption(long id, ApiCreateProductOption data, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new CreateProductOption(id, data), cancellationToken));
    }

    [HttpPut("{id}/options/{optionId}")]
    public async Task<ActionResult<OptionDto>> UpdateProductOption(long id, string optionId, ApiUpdateProductOption data, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new UpdateProductOption(id, optionId, data), cancellationToken));
    }

    [HttpDelete("{id}/options/{optionId}")]
    public async Task<ActionResult> DeleteProductOption(long id, string optionId)
    {
        await _mediator.Send(new DeleteProductOption(id, optionId));
        return Ok();
    }

    [HttpPost("{id}/options/{optionId}/values")]
    public async Task<ActionResult<OptionValueDto>> CreateProductOptionValue(long id, string optionId, ApiCreateProductOptionValue data, CancellationToken cancellationToken)
    {

        return Ok(await _mediator.Send(new CreateProductOptionValue(id, optionId, data), cancellationToken));
    }

    [HttpPost("{id}/options/{optionId}/values/{valueId}")]
    public async Task<ActionResult> DeleteProductOptionValue(long id, string optionId, string valueId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteProductOptionValue(id, optionId, valueId), cancellationToken);
        return Ok();
    }

    [HttpGet("{id}/options/{optionId}/values")]
    public async Task<ActionResult<IEnumerable<OptionValueDto>>> GetProductOptionValues(long id, string optionId)
    {
        return Ok(await _mediator.Send(new GetOptionValues(optionId)));
    }

    [HttpGet("{id}/options/groups")]
    public async Task<ActionResult<IEnumerable<OptionGroupDto>>> GetOptionGroups(long id)
    {
        return Ok(await _mediator.Send(new GetProductOptionGroups(id)));
    }

    [HttpPost("{id}/options/groups")]
    public async Task<ActionResult<OptionGroupDto>> CreateOptionGroup(long id, ApiCreateProductOptionGroup data)
    {
        return Ok(await _mediator.Send(new CreateProductOptionGroup(id, data)));
    }

    [HttpPut("{id}/options/groups/{optionGroupId}")]
    public async Task<ActionResult<OptionGroupDto>> UpdateOptionGroup(long id, string optionGroupId, ApiUpdateProductOptionGroup data)
    {
        return Ok(await _mediator.Send(new UpdateProductOptionGroup(id, optionGroupId, data)));
    }

    [HttpDelete("{id}/options/groups/{optionGroupId}")]
    public async Task<ActionResult> DeleteOptionGroup(long id, string optionGroupId)
    {
        await _mediator.Send(new DeleteProductOptionGroup(id, optionGroupId));
        return Ok();
    }
}