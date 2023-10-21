using Catalog.API.Features.Currencies;
using Catalog.API.Model;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Features.Currencies;

[ApiController]
[Route("api/currencies")]
public class CurrenciesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CurrenciesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<PagedResult<CurrencyDto>> GetCurrencies(int page = 1, int pageSize = 10, string? searchString = null, string? sortBy = null, API.SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new GetCurrenciesQuery(page, pageSize, searchString, sortBy, sortDirection), cancellationToken);
    }
}