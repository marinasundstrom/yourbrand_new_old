using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

using Catalog.API.Persistence;

using MassTransit;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Features.ProductManagement.Products;

public sealed record UpdateProductImage(string IdOrHandle, Stream Stream, string FileName, string ContentType) : IRequest<Result<string>>
{
    public sealed class Handler(BlobServiceClient blobServiceClient, IConfiguration configuration, IPublishEndpoint publishEndpoint, CatalogContext catalogContext = default!) : IRequestHandler<UpdateProductImage, Result<string>>
    {
        public async Task<Result<string>> Handle(UpdateProductImage request, CancellationToken cancellationToken)
        {
            var isId = int.TryParse(request.IdOrHandle, out var id);

            var product = isId ?
                await catalogContext.Products.FirstOrDefaultAsync(product => product.Id == id, cancellationToken)
                : await catalogContext.Products.FirstOrDefaultAsync(product => product.Handle == request.IdOrHandle, cancellationToken);

            if (product is null)
            {
                return Result.Failure<string>(Errors.ProductNotFound);
            }

            var blobContainerClient = blobServiceClient.GetBlobContainerClient("images");
            await blobContainerClient.CreateIfNotExistsAsync();

            BlobClient blobClient = blobContainerClient.GetBlobClient($"products/{request.FileName}");

            await blobClient.UploadAsync(request.Stream, new BlobHttpHeaders { ContentType = request.ContentType });

            var connectionString = catalogContext.Database.GetConnectionString()!;

            string cdnBaseUrl = connectionString.Contains("localhost")
                ? configuration["CdnBaseUrl"]!
                : "https://yourbrandstorage.blob.core.windows.net";

            product.Image = $"{cdnBaseUrl}/images/products/{request.FileName}";

            await catalogContext.SaveChangesAsync(cancellationToken);

            await publishEndpoint.Publish(new Catalog.Contracts.ProductImageUpdated
            {
                ProductId = product.Id,
                ImageUrl = product.Image
            });

            return Result.Success(product.Image);
        }
    }
}
