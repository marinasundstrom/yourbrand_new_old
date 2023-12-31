using YourBrand.Catalog.API.Common;
using YourBrand.Catalog.API.Domain.Entities;
using YourBrand.Catalog.API.Model;
using YourBrand.Catalog.API.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.API.Features.Brands.Queries;

public sealed record GetBrandsQuery(int Page = 0, int PageSize = 10, string? SearchString = null, string? SortBy = null, API.SortDirection? SortDirection = null) : IRequest<PagedResult<BrandDto>>
{
    sealed class GetBrandsQueryHandler : IRequestHandler<GetBrandsQuery, PagedResult<BrandDto>>
    {
        private readonly CatalogContext _context;
        private readonly ICurrentUserService currentUserService;

        public GetBrandsQueryHandler(
            CatalogContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            this.currentUserService = currentUserService;
        }

        public async Task<PagedResult<BrandDto>> Handle(GetBrandsQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Brand> result = _context
                    .Brands
                     //.OrderBy(o => o.Created)
                     .AsNoTracking()
                     .AsQueryable();

            if (request.SearchString is not null)
            {
                result = result.Where(ca => ca.Name.ToLower().Contains(request.SearchString.ToLower()));
            }

            var totalCount = await result.CountAsync(cancellationToken);

            if (request.SortBy is not null)
            {
                result = result.OrderBy(request.SortBy, request.SortDirection == API.SortDirection.Desc ? API.SortDirection.Desc : API.SortDirection.Asc);
            }
            else
            {
                result = result.OrderBy(x => x.Name);
            }

            var items = await result
                .Skip((request.Page) * request.PageSize)
                .Take(request.PageSize)
                .ToArrayAsync(cancellationToken);

            return new PagedResult<BrandDto>(items.Select(cp => cp.ToDto()), totalCount);
        }
    }
}