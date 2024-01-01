using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

using YourBrand.Catalog.API.Persistence;

using MassTransit;

using MediatR;

using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace YourBrand.Catalog.API.Features.ProductManagement.Products.Images;

public sealed record UploadProductImage(string IdOrHandle, Stream Stream, string FileName, string ContentType) : IRequest<Result<ProductImageDto>>
{
    public sealed class Handler(IProductImageUploader productImageUploader, IPublishEndpoint publishEndpoint, CatalogContext catalogContext = default!) : IRequestHandler<UploadProductImage, Result<ProductImageDto>>
    {
        public async Task<Result<ProductImageDto>> Handle(UploadProductImage request, CancellationToken cancellationToken)
        {
            var isId = int.TryParse(request.IdOrHandle, out var id);

            var product = isId ?
                await catalogContext.Products.IncludeImages().FirstOrDefaultAsync(product => product.Id == id, cancellationToken)
                : await catalogContext.Products.IncludeImages().FirstOrDefaultAsync(product => product.Handle == request.IdOrHandle, cancellationToken);

            if (product is null)
            {
                return Result.Failure<ProductImageDto>(Errors.ProductNotFound);
            }

            product.Image = await productImageUploader.UploadProductImage(product.Id, request.FileName, request.Stream, request.ContentType);

            var image = new Domain.Entities.ProductImage(request.FileName, string.Empty, product.Image);

            product.AddImage(image);

            await catalogContext.SaveChangesAsync(cancellationToken);

            await publishEndpoint.Publish(new Catalog.Contracts.ProductImageUpdated
            {
                ProductId = product.Id,
                ImageUrl = product.Image
            });

            return Result.Success(image.ToDto());
        }
    }
}
