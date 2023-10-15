using Catalog.API;
using Catalog.API.Model;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Catalog.API.Features.ProductManagement.Attributes;
using Catalog.API.Features.ProductManagement.Attributes.Groups;
using Catalog.API.Features.ProductManagement.Attributes.Values;
using Catalog.API.Features.ProductManagement.Options;

namespace Catalog.API.Features.ProductManagement.Options;

[ApiController]
//[ApiVersion("1")]
[Route("api/attributes")] // v{version:apiVersion}/
public class AttributesController : Controller
{
    private readonly IMediator _mediator;

    public AttributesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<AttributeDto>>> GetAttributes(
        [FromQuery] string[]? ids = null, int page = 0, int pageSize = 10, string? searchString = null, string? sortBy = null, SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new GetAttributes(ids, page, pageSize, searchString, sortBy, sortDirection), cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<AttributeDto> GetAttribute(string id, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new GetAttribute(id));
    }

    [HttpGet("{id}/values")]
    public async Task<ActionResult<OptionValueDto>> GetAttributesValues(string id, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetAttributeValues(id), cancellationToken));
    }
    [HttpPost]
    public async Task<AttributeDto> CreateAttribute(CreateAttributeDto dto, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new CreateAttributeCommand(dto.Name, dto.Description, dto.GroupId, dto.Values), cancellationToken);
    }

    [HttpPut("{id}")]
    public async Task UpdateAttribute(string id, UpdateAttributeDto dto, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateAttributeCommand(id, dto.Name, dto.Description, dto.GroupId, dto.Values), cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAttribute(string id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteAttributeCommand(id), cancellationToken);
    }

    [HttpPost("{id}/values")]
    public async Task<ActionResult<AttributeValueDto>> CreateAttributeValue(string id, ApiCreateProductAttributeValue data, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new CreateProductAttributeValue(id, data), cancellationToken));
    }

    [HttpPost("{id}/values/{valueId}")]
    public async Task<ActionResult> DeleteAttributeValue(string id, string valueId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteProductAttributeValue(id, valueId), cancellationToken);
        return Ok();
    }

    [HttpGet("Groups")]
    public async Task<ActionResult<IEnumerable<AttributeGroupDto>>> GetAttributeGroups()
    {
        return Ok(await _mediator.Send(new GetAttributeGroups()));
    }

    [HttpPost("groups")]
    public async Task<ActionResult<AttributeGroupDto>> CreateAttributeGroup(ApiCreateProductAttributeGroup data)
    {
        return Ok(await _mediator.Send(new CreateAttributeGroup(data)));
    }

    [HttpPut("groups/{id}")]
    public async Task<ActionResult<AttributeGroupDto>> UpdateAttributeGroup(string id, ApiUpdateProductAttributeGroup data)
    {
        return Ok(await _mediator.Send(new UpdateAttributeGroup(id, data)));
    }

    [HttpDelete("groups/{id}")]
    public async Task<ActionResult> DeleteAttributeGroup(string id)
    {
        await _mediator.Send(new DeleteAttributeGroup(id));
        return Ok();
    }

}

public record CreateAttributeDto(string Name, string? Description, string? GroupId, IEnumerable<ApiCreateProductAttributeValue> Values);

public record UpdateAttributeDto(string Name, string? Description, string? GroupId, IEnumerable<ApiUpdateProductAttributeValue> Values);