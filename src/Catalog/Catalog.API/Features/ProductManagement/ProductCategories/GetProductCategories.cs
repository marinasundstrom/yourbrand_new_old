using YourBrand.Catalog.API.Model;
using YourBrand.Catalog.API.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.API.Features.ProductManagement.ProductCategories;

public sealed record GetProductCategories(string? StoreId, long? ParentGroupId, bool IncludeWithUnlistedProducts, bool IncludeHidden,
    int Page = 1, int PageSize = 10, string? SearchTerm = null, string? SortBy = null, SortDirection? SortDirection = null) : IRequest<PagedResult<ProductCategory>>
{
    public sealed class Handler(CatalogContext catalogContext = default!) : IRequestHandler<GetProductCategories, PagedResult<ProductCategory>>
    {
        public async Task<PagedResult<ProductCategory>> Handle(GetProductCategories request, CancellationToken cancellationToken)
        {
            var query = catalogContext.ProductCategories.AsNoTracking().AsQueryable();

            if (request.StoreId is not null)
            {
                query = query.Where(x => x.StoreId == request.StoreId);
            }

            if (request.ParentGroupId is not null)
            {
                query = query.Where(x => x.Parent!.Id == request.ParentGroupId);
            }

            /*
            if (!request.IncludeHidden)
            {
                query = query.Where(x => !x.Hidden);
            }
            */

            var doNotIncludeWithUnlisted = !request.IncludeWithUnlistedProducts;
            if (doNotIncludeWithUnlisted)
            {
                query = query.Where(x => x.Products.Any()
                && !x.Products.All(z => z.Visibility == Domain.Enums.ProductVisibility.Unlisted));
            }

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