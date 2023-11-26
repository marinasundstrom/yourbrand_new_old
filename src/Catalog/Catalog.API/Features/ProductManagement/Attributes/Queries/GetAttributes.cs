using Catalog.API.Model;
using Catalog.API.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Features.ProductManagement.Attributes;

public record GetAttributes(string[]? Ids = null, int Page = 1, int PageSize = 10, string? SearchString = null, string? SortBy = null, SortDirection? SortDirection = null) : IRequest<PagedResult<AttributeDto>>
{
    public class Handler : IRequestHandler<GetAttributes, PagedResult<AttributeDto>>
    {
        private readonly CatalogContext _context;

        public Handler(CatalogContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<AttributeDto>> Handle(GetAttributes request, CancellationToken cancellationToken)
        {
            var query = _context.Attributes
                .AsSplitQuery()
                .AsNoTracking()
                .Include(o => o.Group)
                .Include(o => o.Values)
                .AsQueryable();

            if (request.Ids?.Any() ?? false)
            {
                var ids = request.Ids;
                query = query.Where(o => ids.Any(x => x == o.Id));
            }

            if (request.SearchString is not null)
            {
                query = query.Where(o => o.Name.ToLower().Contains(request.SearchString.ToLower()));
            }

            if (request.SortBy is not null)
            {
                query = query.OrderBy(request.SortBy, request.SortDirection == API.SortDirection.Desc ? API.SortDirection.Desc : API.SortDirection.Asc);
            }
            else
            {
                query = query.OrderBy(x => x.Name);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                            .OrderBy(x => x.Name)
                            .Include(x => x.Values)
                            .Skip(request.PageSize * (request.Page - 1))
                            .Take(request.PageSize).AsQueryable()
                            .ToArrayAsync();

            return new PagedResult<AttributeDto>(items.Select(item => item.ToDto()), totalCount);
        }
    }
}