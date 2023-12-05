using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

using Catalog.API.Persistence;

using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Features.ProductManagement.Products;

public interface IProductImageUploader
{
    Task<string> GetPlaceholderImageUrl();
    Task<bool> TryDeleteProductImage(long productId, string fileName);
    Task<string> UploadProductImage(long productId, string fileName, Stream stream, string contentType);
}

public class ProductImageUploader(BlobServiceClient blobServiceClient, CatalogContext context, IConfiguration configuration)
    : IProductImageUploader
{
    private string? placeholderImageFileName;

    public Task<string> GetPlaceholderImageUrl()
    {
        if (placeholderImageFileName is null)
        {
            string fileName = "placeholder.jpeg";

            var connectionString = context.Database.GetConnectionString()!;

            string cdnBaseUrl = (connectionString.Contains("localhost") || connectionString.Contains("mssql"))
                ? configuration["CdnBaseUrl"]!
                : "https://yourbrandstorage.blob.core.windows.net";

            placeholderImageFileName = $"{cdnBaseUrl}/images/products/{fileName}";
        }

        return Task.FromResult(placeholderImageFileName);
    }

    public async Task<bool> TryDeleteProductImage(long productId, string fileName)
    {
        var blobContainerClient = blobServiceClient.GetBlobContainerClient("images");
        await blobContainerClient.CreateIfNotExistsAsync();

        BlobClient blobClient = blobContainerClient.GetBlobClient($"products/{productId}/{fileName}");

        return await blobClient.DeleteIfExistsAsync();
    }

    public async Task<string> UploadProductImage(long productId, string fileName, Stream stream, string contentType)
    {
        var blobContainerClient = blobServiceClient.GetBlobContainerClient("images");
        await blobContainerClient.CreateIfNotExistsAsync();

        BlobClient blobClient = blobContainerClient.GetBlobClient($"products/{productId}/{fileName}");

        await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = contentType });

        var connectionString = context.Database.GetConnectionString()!;

        string cdnBaseUrl = (connectionString.Contains("localhost") || connectionString.Contains("mssql"))
            ? configuration["CdnBaseUrl"]!
            : "https://yourbrandstorage.blob.core.windows.net";

        return $"{cdnBaseUrl}/images/products/{productId}/{fileName}";
    }
}