
using Catalog.API.Features.Brands.Commands;
using Catalog.API.Features.Brands.Queries;
using Catalog.API.Model;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Features.Brands;

[ApiController]
[Route("api/brands")]
public class BrandsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BrandsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<PagedResult<BrandDto>> GetBrands(int page = 1, int pageSize = 10, string? searchString = null, string? sortBy = null, API.SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new GetBrandsQuery(page - 1, pageSize, searchString, sortBy, sortDirection), cancellationToken);
    }

    [HttpGet("{id}")]
    public async Task<BrandDto?> GetBrand(int id, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new GetBrandQuery(id), cancellationToken);
    }

    [HttpPost]
    public async Task<BrandDto> CreateBrand(CreateBrandDto dto, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new CreateBrandCommand(dto.Name, dto.Handle), cancellationToken);
    }

    [HttpPut("{id}")]
    public async Task UpdateBrand(int id, UpdateBrandDto dto, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateBrandCommand(id, dto.Name, dto.Handle), cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task DeleteBrand(int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteBrandCommand(id), cancellationToken);
    }
}

public record CreateBrandDto(string Name, string Handle);

public record UpdateBrandDto(string Name, string Handle);