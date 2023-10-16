using Catalog.API.Features.Stores;
using Catalog.API.Features.Stores.Commands;
using Catalog.API.Features.Stores.Queries;
using Catalog.API.Model;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Features.Stores;

[ApiController]
[Route("api/stores")]
public class StoresController : ControllerBase
{
    private readonly IMediator _mediator;

    public StoresController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<PagedResult<StoreDto>> GetStores(int page = 1, int pageSize = 10, string? searchString = null, string? sortBy = null, API.SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new GetStoresQuery(page, pageSize, searchString, sortBy, sortDirection), cancellationToken);
    }

    [HttpGet("{IdOrHandle}")]
    public async Task<StoreDto?> GetStore(string IdOrHandle, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new GetStoreQuery(IdOrHandle), cancellationToken);
    }

    [HttpPost]
    public async Task<StoreDto> CreateStore(CreateStoreDto dto, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new CreateStoreCommand(dto.Name, dto.Handle, dto.Currency), cancellationToken);
    }

    [HttpPut("{id}")]
    public async Task UpdateStore(string id, UpdateStoreDto dto, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateStoreCommand(id, dto.Name, dto.Handle, dto.Currency), cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task DeleteStore(string id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteStoreCommand(id), cancellationToken);
    }
}

public record CreateStoreDto(string Name, string Handle, string Currency);

public record UpdateStoreDto(string Name, string Handle, string Currency);