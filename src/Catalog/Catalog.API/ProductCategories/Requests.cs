using System.Net;
using Catalog.API.Data;
using Catalog.API.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.ProductCategories;

public static class Errors
{
    public readonly static Error ProductCategoryNotFound = new("product-category-not-found", "Category not found", "");

    public readonly static Error HandleAlreadyTaken = new("handle-already-taken", "Handle already taken", "");
}

public sealed record GetProductCategories(int Page = 1, int PageSize = 10, string? SearchTerm = null, string? CategoryPath = null) : IRequest<PagedResult<ProductCategory>>
{
    public sealed class Handler(CatalogContext catalogContext = default!) : IRequestHandler<GetProductCategories, PagedResult<ProductCategory>>
    {
        public async Task<PagedResult<ProductCategory>> Handle(GetProductCategories request, CancellationToken cancellationToken)
        {
            var query = catalogContext.ProductCategories.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                query = query.Where(x => x.Name.ToLower().Contains(request.SearchTerm.ToLower()!) || x.Description.ToLower().Contains(request.SearchTerm.ToLower()!));
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

public sealed record GetProductCategoryById(string IdOrPath) : IRequest<Result<ProductCategory>>
{
    public sealed class Handler(CatalogContext catalogContext = default!) : IRequestHandler<GetProductCategoryById, Result<ProductCategory>>
    {
        public async Task<Result<ProductCategory>> Handle(GetProductCategoryById request, CancellationToken cancellationToken)
        {
            var idOrPath = request.IdOrPath;
            var isId = int.TryParse(request.IdOrPath, out var id);

            Model.ProductCategory? productCategory;

            if (isId)
            {
                productCategory = await catalogContext.ProductCategories
                    .Include(x => x!.Parent)
                    .FirstOrDefaultAsync(productCategory => productCategory.Id == id, cancellationToken);
            }
            else
            {
                idOrPath = WebUtility.UrlDecode(idOrPath);

                productCategory = await catalogContext.ProductCategories
                     .Include(x => x!.Parent)
                     .FirstOrDefaultAsync(productCategory => productCategory.Path == idOrPath, cancellationToken);
            }
            return productCategory is null
                ? Result.Failure<ProductCategory>(Errors.ProductCategoryNotFound)
                : Result.Success(productCategory.ToDto());
        }
    }
}

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

public sealed record CreateProductCategory(string Name, string Description, long ParentCategoryId, string Handle) : IRequest<Result<ProductCategory>>
{
    public sealed class Handler(IConfiguration configuration, CatalogContext catalogContext = default!) : IRequestHandler<CreateProductCategory, Result<ProductCategory>>
    {
        public async Task<Result<ProductCategory>> Handle(CreateProductCategory request, CancellationToken cancellationToken)
        {
            var parentCategory = await catalogContext.ProductCategories
                .FirstOrDefaultAsync(p => p.Id == request.ParentCategoryId, cancellationToken);

            var product = new Model.ProductCategory()
            {
                Name = request.Name,
                Description = request.Description,
                Parent = parentCategory,
                Handle = request.Handle,
                Path = $"{parentCategory!.Path}/{request.Handle}"
            };

            catalogContext.ProductCategories.Add(product);

            await catalogContext.SaveChangesAsync(cancellationToken);

            return Result.Success(product.ToDto());
        }
    }
}

public sealed record UpdateProductCategoryDetails(string IdOrPath, string Name, string Description) : IRequest<Result>
{
    public sealed class Handler(CatalogContext catalogContext = default!) : IRequestHandler<UpdateProductCategoryDetails, Result>
    {
        public async Task<Result> Handle(UpdateProductCategoryDetails request, CancellationToken cancellationToken)
        {
            var isId = int.TryParse(request.IdOrPath, out var id);

            var product = isId ?
                await catalogContext.ProductCategories.FirstOrDefaultAsync(product => product.Id == id, cancellationToken)
                : await catalogContext.ProductCategories.FirstOrDefaultAsync(product => product.Path == request.IdOrPath, cancellationToken);

            if (product is null)
            {
                return Result.Failure(Errors.ProductCategoryNotFound);
            }

            product.Name = request.Name;
            product.Description = request.Description;

            await catalogContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}

public sealed record DeleteProductCategory(string IdOrPath) : IRequest<Result>
{
    public sealed class Handler(CatalogContext catalogContext = default!) : IRequestHandler<DeleteProductCategory, Result>
    {
        public async Task<Result> Handle(DeleteProductCategory request, CancellationToken cancellationToken)
        {
            var isId = int.TryParse(request.IdOrPath, out var id);

            var product = isId ?
                await catalogContext.ProductCategories.FirstOrDefaultAsync(product => product.Id == id, cancellationToken)
                : await catalogContext.ProductCategories.FirstOrDefaultAsync(product => product.Path == request.IdOrPath, cancellationToken);

            if (product is null)
            {
                return Result.Failure(Errors.ProductCategoryNotFound);
            }

            catalogContext.ProductCategories.Remove(product);

            await catalogContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}