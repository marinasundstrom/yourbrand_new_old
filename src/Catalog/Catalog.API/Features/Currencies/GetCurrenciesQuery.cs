using Catalog.API;
using Catalog.API.Common;
using Catalog.API.Features.Currencies;
using Catalog.API.Model;
using Catalog.API.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Features.Currencies;

public sealed record GetCurrenciesQuery(int Page = 0, int PageSize = 10, string? SearchString = null, string? SortBy = null, API.SortDirection? SortDirection = null) : IRequest<PagedResult<CurrencyDto>>
{
    sealed class GetCurrenciesQueryHandler : IRequestHandler<GetCurrenciesQuery, PagedResult<CurrencyDto>>
    {
        private readonly CatalogContext _context;
        private readonly ICurrentUserService currentUserService;

        public GetCurrenciesQueryHandler(
            CatalogContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            this.currentUserService = currentUserService;
        }

        public async Task<PagedResult<CurrencyDto>> Handle(GetCurrenciesQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Currencies
                .AsSplitQuery()
                .AsNoTracking()
                .AsQueryable();

            if (request.SearchString is not null)
            {
                query = query.Where(ca => ca.Name.ToLower().Contains(request.SearchString.ToLower()));
            }

            var totalCount = await query.CountAsync();

            if (request.SortBy is not null)
            {
                query = query.OrderBy(request.SortBy, request.SortDirection == API.SortDirection.Desc ? Catalog.API.SortDirection.Desc : Catalog.API.SortDirection.Asc);
            }
            else
            {
                query = query.OrderBy(x => x.Symbol);
            }

            var items = await query
                .Skip(request.Page * request.PageSize)
                .Take(request.PageSize).AsQueryable()
                .ToArrayAsync();

            return new PagedResult<CurrencyDto>(items.Select(item => item.ToDto()),
            totalCount);
        }
    }
}