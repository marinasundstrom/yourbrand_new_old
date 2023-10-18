using Catalog.API.Model;
using Catalog.API.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Features.ProductManagement.Products;

public sealed record GetProducts(string? StoreId = null, string? BrandIdOrHandle = null, bool IncludeUnlisted = false, bool GroupProducts = true, string? ProductCategoryIdOrPath = null, int Page = 10, int PageSize = 10, string? SearchTerm = null, string? SortBy = null, API.SortDirection? SortDirection = null) : IRequest<PagedResult<ProductDto>>
{
    public sealed class Handler(CatalogContext catalogContext = default!) : IRequestHandler<GetProducts, PagedResult<ProductDto>>
    {
        public async Task<PagedResult<ProductDto>> Handle(GetProducts request, CancellationToken cancellationToken)
        {
            var query = catalogContext.Products
                        .Where(x => x.Category != null)
                        .IncludeAll()
                        .AsSplitQuery()
                        .AsNoTracking()
                        .AsQueryable();

            if (request.StoreId is not null)
            {
                query = query.Where(x => x.StoreId == request.StoreId);
            }

            if (request.BrandIdOrHandle is not null)
            {
                bool isBrandId = long.TryParse(request.BrandIdOrHandle, out var brandId);

                query = isBrandId ?
                    query.Where(pv => pv.BrandId == brandId)
                    : query.Where(pv => pv.Brand!.Handle == request.BrandIdOrHandle);
            }

             if (!request.IncludeUnlisted)
            {
                query = query.Where(x => x.Visibility == Domain.Enums.ProductVisibility.Listed);
            }

            if (!string.IsNullOrEmpty(request.ProductCategoryIdOrPath))
            {
                bool isProductCategoryId = long.TryParse(request.ProductCategoryIdOrPath, out var categoryId);

                query = isProductCategoryId 
                            ? query.Where(x => x.Category!.Id == categoryId)
                            : query.Where(x => 
                                x.Category!.Path.StartsWith(request.ProductCategoryIdOrPath));
            }

            if (request.GroupProducts)
            {
                query = query.Where(x => x.ParentProductId == null);
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

            return new PagedResult<ProductDto>(products.Select(x => x.ToDto()), total);
        }
    }
}