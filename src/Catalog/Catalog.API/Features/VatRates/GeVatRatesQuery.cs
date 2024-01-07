using YourBrand.Catalog.API;
using YourBrand.Catalog.API.Common;
using YourBrand.Catalog.API.Features.VatRates;
using YourBrand.Catalog.API.Model;
using YourBrand.Catalog.API.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.API.Features.VatRates;

public sealed record GetVatRatesQuery(int Page = 1, int PageSize = 10, string? SearchString = null, string? SortBy = null, API.SortDirection? SortDirection = null) : IRequest<PagedResult<VatRateDto>>
{
    sealed class GetVatRatesQueryHandler : IRequestHandler<GetVatRatesQuery, PagedResult<VatRateDto>>
    {
        private readonly CatalogContext _context;
        private readonly ICurrentUserService currentUserService;

        public GetVatRatesQueryHandler(
            CatalogContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            this.currentUserService = currentUserService;
        }

        public async Task<PagedResult<VatRateDto>> Handle(GetVatRatesQuery request, CancellationToken cancellationToken)
        {
            var query = _context.VatRates
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
                query = query.OrderBy(x => x.Name);
            }

            var items = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize).AsQueryable()
                .ToArrayAsync();

            return new PagedResult<VatRateDto>(items.Select(item => item.ToDto()),
            totalCount);
        }
    }
}