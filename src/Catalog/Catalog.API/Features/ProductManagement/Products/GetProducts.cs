using Catalog.API.Model;
using Catalog.API.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Features.ProductManagement.Products;

public sealed record GetProducts(int Page = 1, int PageSize = 10, string? SearchTerm = null, string? CategoryPath = null, string? SortBy = null, SortDirection? SortDirection = null) : IRequest<PagedResult<Product>>
{
    public sealed class Handler(CatalogContext catalogContext = default!) : IRequestHandler<GetProducts, PagedResult<Product>>
    {
        public async Task<PagedResult<Product>> Handle(GetProducts request, CancellationToken cancellationToken)
        {
            var query = catalogContext.Products
                        .Include(x => x.Category)
                        .ThenInclude(x => x.Parent)
                        .AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(request.CategoryPath))
            {
                query = query.Where(x => x.Category.Path.StartsWith(request.CategoryPath));
            }

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                query = query.Where(x => x.Name.ToLower().Contains(request.SearchTerm.ToLower()!) || x.Description.ToLower().Contains(request.SearchTerm.ToLower()!));
            }

            var total = await query.CountAsync(cancellationToken);

            if (request.SortBy is null)
            {
                query = query.OrderBy(x => x.Name);
            }
            else
            {
                query = query.OrderBy(request.SortBy, request.SortDirection);
            }

            var products = await query
                .Skip(request.PageSize * (request.Page - 1))
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<Product>(products.Select(x => x.ToDto()), total);
        }
    }
}
