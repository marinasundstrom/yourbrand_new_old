using YourBrand.Catalog.API.Persistence;

using MassTransit;

using MediatR;

using Microsoft.EntityFrameworkCore;
using YourBrand.Catalog.API.Domain.Entities;

namespace YourBrand.Catalog.API.Features.ProductManagement.Products.Images;

public sealed record DeleteProductImage(string IdOrHandle, string ProductImageId) : IRequest<Result<ProductImageDto>>
{
    public sealed class Handler(IProductImageUploader productImageUploader, IPublishEndpoint publishEndpoint, CatalogContext catalogContext = default!) : IRequestHandler<DeleteProductImage, Result<ProductImageDto>>
    {
        public async Task<Result<ProductImageDto>> Handle(DeleteProductImage request, CancellationToken cancellationToken)
        {
            var isId = int.TryParse(request.IdOrHandle, out var id);

            var product = isId ?
                await catalogContext.Products.IncludeImages().FirstOrDefaultAsync(product => product.Id == id, cancellationToken)
                : await catalogContext.Products.IncludeImages().FirstOrDefaultAsync(product => product.Handle == request.IdOrHandle, cancellationToken);

            if (product is null)
            {
                return Result.Failure<ProductImageDto>(Errors.ProductNotFound);
            }

            var productImage = product.Images.FirstOrDefault(x => x.Id == request.ProductImageId);

            // Delete image

            product.RemoveImage(productImage);

            await catalogContext.SaveChangesAsync(cancellationToken);

            await publishEndpoint.Publish(new Catalog.Contracts.ProductImageUpdated
            {
                ProductId = product.Id,
                ImageUrl = product.Image.Url
            });

            return Result.Success(product.Image.ToDto());
        }
    }
}
