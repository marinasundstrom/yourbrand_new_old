using Catalog.API.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Features.ProductManagement.ProductCategories;

public sealed record CreateProductCategory(string Name, string Description, long ParentCategoryId, string Handle) : IRequest<Result<ProductCategory>>
{
    public sealed class Handler(IConfiguration configuration, CatalogContext catalogContext = default!) : IRequestHandler<CreateProductCategory, Result<ProductCategory>>
    {
        public async Task<Result<ProductCategory>> Handle(CreateProductCategory request, CancellationToken cancellationToken)
        {
            var parentCategory = await catalogContext.ProductCategories
                .FirstOrDefaultAsync(p => p.Id == request.ParentCategoryId, cancellationToken);

            var product = new Domain.Entities.ProductCategory()
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