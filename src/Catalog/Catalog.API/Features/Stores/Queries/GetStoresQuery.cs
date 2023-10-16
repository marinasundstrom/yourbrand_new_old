using Catalog.API;
using Catalog.API.Common;
using Catalog.API.Features.Stores;
using Catalog.API.Model;
using Catalog.API.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Features.Stores.Queries;

public sealed record GetStoresQuery(int Page = 0, int PageSize = 10, string? SearchString = null, string? SortBy = null, API.SortDirection? SortDirection = null) : IRequest<PagedResult<StoreDto>>
{
    sealed class GetStoresQueryHandler : IRequestHandler<GetStoresQuery, PagedResult<StoreDto>>
    {
        private readonly CatalogContext _context;
        private readonly ICurrentUserService currentUserService;

        public GetStoresQueryHandler(
            CatalogContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            this.currentUserService = currentUserService;
        }

        public async Task<PagedResult<StoreDto>> Handle(GetStoresQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Stores
                .Include(x => x.Currency)
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
                .Include(x => x.Currency)
                .Skip(request.Page * request.PageSize)
                .Take(request.PageSize).AsQueryable()
                .ToArrayAsync();

            return new PagedResult<StoreDto>(items.Select(item => item.ToDto()),
            totalCount);
        }
    }
}