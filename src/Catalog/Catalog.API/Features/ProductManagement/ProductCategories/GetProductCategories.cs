using Catalog.API.Model;
using Catalog.API.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Features.ProductManagement.ProductCategories;

public sealed record GetProductCategories(int Page = 1, int PageSize = 10, string? SearchTerm = null, string? CategoryPath = null) : IRequest<PagedResult<ProductCategory>>
{
    public sealed class Handler(CatalogContext catalogContext = default!) : IRequestHandler<GetProductCategories, PagedResult<ProductCategory>>
    {
        public async Task<PagedResult<ProductCategory>> Handle(GetProductCategories request, CancellationToken cancellationToken)
        {
            var query = catalogContext.ProductCategories.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                var t = $"%{request.SearchTerm}%";
                query = query.Where(x => EF.Functions.Like(x.Name, t) || EF.Functions.Like(x.Description, t));
            }

            var total = await query.CountAsync(cancellationToken);

            var productCategories = await query.OrderBy(x => x.Name)
                .Skip(request.PageSize * (request.Page - 1))
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<ProductCategory>(productCategories.Select(x => x.ToDto()), total);
        }
    }
}