using Catalog.API.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Features.ProductManagement.ProductCategories;

public sealed record GetProductCategoryTree() : IRequest<Result<ProductCategoryTreeRootDto>>
{
    public sealed class Handler(CatalogContext catalogContext = default!) : IRequestHandler<GetProductCategoryTree, Result<ProductCategoryTreeRootDto>>
    {
        public async Task<Result<ProductCategoryTreeRootDto>> Handle(GetProductCategoryTree request, CancellationToken cancellationToken)
        {
            var query = catalogContext.ProductCategories
            .Include(x => x.Parent)
            .ThenInclude(x => x!.Parent)
            .Include(x => x.SubCategories.OrderBy(x => x.Name))
            .Where(x => x.Parent == null)
            .OrderBy(x => x.Name)
            .AsSingleQuery()
            .AsNoTracking();

            var itemGroups = await query
                .ToArrayAsync(cancellationToken);

            var root = new ProductCategoryTreeRootDto(itemGroups.Select(x => x.ToProductCategoryTreeNodeDto()), itemGroups.Sum(x => x.ProductsCount));

            return Result.Success(root);
        }
    }
}
