using YourBrand.Catalog.API.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;
using YourBrand.Catalog.API.Features.ProductManagement.Products.Images;
using YourBrand.Catalog.API.Domain.Entities;

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
                HasVariants = request.IsGroupedProduct,
                Price = request.Price,
                VatRate = request.VatRate,
                Handle = request.Handle
            };

            var image = new ProductImage("Placeholder", string.Empty, await productImageUploader.GetPlaceholderImageUrl());
            product.AddImage(image);

            var category = await catalogContext.ProductCategories
                .Include(x => x.Parent)
                .FirstAsync(x => x.Id == request.CategoryId, cancellationToken);

            category.AddProduct(product);

            catalogContext.Products.Add(product);

            await catalogContext.SaveChangesAsync(cancellationToken);

            product.Image = image;

            await catalogContext.SaveChangesAsync(cancellationToken);

            return Result.Success(product.ToDto());
        }
    }
}