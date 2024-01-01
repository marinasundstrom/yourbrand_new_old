using YourBrand.Catalog.API.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;
using YourBrand.Catalog.API.Features.ProductManagement.Products.Images;

namespace YourBrand.Catalog.API.Features.ProductManagement.Products;

public sealed record CreateProduct(string Name, string StoreId, string Description, long CategoryId, bool IsGroupedProduct, decimal Price, double? VatRate, string Handle) : IRequest<Result<ProductDto>>
{
    public sealed class Handler(CatalogContext catalogContext, IProductImageUploader productImageUploader) : IRequestHandler<CreateProduct, Result<ProductDto>>
    {
        public async Task<Result<ProductDto>> Handle(CreateProduct request, CancellationToken cancellationToken)
        {
            var handleInUse = await catalogContext.Products.AnyAsync(product => product.Handle == request.Handle, cancellationToken);

            if (handleInUse)
            {
                return Result.Failure<ProductDto>(Errors.HandleAlreadyTaken);
            }

            var product = new Domain.Entities.Product()
            {
                Name = request.Name,
                StoreId = request.StoreId,
                Description = request.Description,
                Image = await productImageUploader.GetPlaceholderImageUrl(),
                HasVariants = request.IsGroupedProduct,
                Price = request.Price,
                VatRate = request.VatRate,
                Handle = request.Handle
            };

            var category = await catalogContext.ProductCategories
                .Include(x => x.Parent)
                .FirstAsync(x => x.Id == request.CategoryId, cancellationToken);

            category.AddProduct(product);

            catalogContext.Products.Add(product);

            await catalogContext.SaveChangesAsync(cancellationToken);

            return Result.Success(product.ToDto());
        }
    }
}