using System;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Catalog.API.Data;
using Catalog.API.Model;
using MassTransit;
using MassTransit.Transports;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Products;

public static class Errors
{
    public readonly static Error ProductNotFound = new("product-not-found", "Product not found", "");

    public readonly static Error HandleAlreadyTaken = new("handle-already-taken", "Handle already taken", "");
}

public sealed record GetProducts(int Page = 1, int PageSize = 10, string? SearchTerm = null, string? CategoryPath = null) : IRequest<PagedResult<Product>>
{
    public sealed class Handler(CatalogContext catalogContext = default!) : IRequestHandler<GetProducts, PagedResult<Product>>
    {
        public async Task<PagedResult<Product>> Handle(GetProducts request, CancellationToken cancellationToken)
        {
            var query = catalogContext.Products
                        .Include(x => x.Category)
                        .ThenInclude(x => x.Parent)
                        .AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(request.CategoryPath))
            {
                query = query.Where(x => x.Category.Path.StartsWith(request.CategoryPath));
            }

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                query = query.Where(x => x.Name.ToLower().Contains(request.SearchTerm.ToLower()!) || x.Description.ToLower().Contains(request.SearchTerm.ToLower()!));
            }

            var total = await query.CountAsync(cancellationToken);

            var products = await query.OrderBy(x => x.Name)
                .Skip(request.PageSize * (request.Page - 1))
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<Product>(products.Select(x => x.ToDto()), total);
        }
    }
}

public sealed record GetProductById(string IdOrHandle) : IRequest<Result<Product>>
{
    public sealed class Handler(CatalogContext catalogContext = default!) : IRequestHandler<GetProductById, Result<Product>>
    {
        public async Task<Result<Product>> Handle(GetProductById request, CancellationToken cancellationToken)
        {
            var isId = int.TryParse(request.IdOrHandle, out var id);

            var query = catalogContext.Products
                .Include(x => x.Category)
                .ThenInclude(x => x.Parent)
                .AsQueryable();

            var product = isId ?
                await query.FirstOrDefaultAsync(product => product.Id == id, cancellationToken)
                : await query.FirstOrDefaultAsync(product => product.Handle == request.IdOrHandle, cancellationToken);

            return product is null
                ? Result.Failure<Product>(Errors.ProductNotFound)
                : Result.Success(product.ToDto());
        }
    }
}

public sealed record CreateProduct(string Name, string Description, long CategoryId, decimal Price, string Handle) : IRequest<Result<Product>>
{
    public sealed class Handler(IConfiguration configuration, CatalogContext catalogContext = default!) : IRequestHandler<CreateProduct, Result<Product>>
    {
        public async Task<Result<Product>> Handle(CreateProduct request, CancellationToken cancellationToken)
        {
            var handleInUse = await catalogContext.Products.AnyAsync(product => product.Handle == request.Handle, cancellationToken);

            if (handleInUse)
            {
                return Result.Failure<Product>(Errors.HandleAlreadyTaken);
            }

            string cdnBaseUrl = configuration["CdnBaseUrl"] ?? "https://yourbrandstorage.blob.core.windows.net";

            var product = new Model.Product()
            {
                Name = request.Name,
                Description = request.Description,
                Image = $"{cdnBaseUrl}/images/products/placeholder.jpeg",
                Price = request.Price,
                Handle = request.Handle,
                Category = await catalogContext.ProductCategories.FirstAsync(x => x.Id == request.CategoryId, cancellationToken)
            };

            catalogContext.Products.Add(product);

            await catalogContext.SaveChangesAsync(cancellationToken);

            return Result.Success(product.ToDto());
        }
    }
}

public sealed record UpdateProductDetails(string IdOrHandle, string Name, string Description) : IRequest<Result>
{
    public sealed class Handler(IPublishEndpoint publishEndpoint, CatalogContext catalogContext = default!) : IRequestHandler<UpdateProductDetails, Result>
    {
        public async Task<Result> Handle(UpdateProductDetails request, CancellationToken cancellationToken)
        {
            var isId = int.TryParse(request.IdOrHandle, out var id);

            var product = isId ?
                await catalogContext.Products.FirstOrDefaultAsync(product => product.Id == id, cancellationToken)
                : await catalogContext.Products.FirstOrDefaultAsync(product => product.Handle == request.IdOrHandle, cancellationToken);

            if (product is null)
            {
                return Result.Failure(Errors.ProductNotFound);
            }

            product.Name = request.Name;
            product.Description = request.Description;

            await catalogContext.SaveChangesAsync(cancellationToken);

            await publishEndpoint.Publish(new Catalog.Contracts.ProductDetailsUpdated
            {
                ProductId = product.Id,
                Name = product.Name,
                Description = product.Description
            });

            return Result.Success();
        }
    }
}

public sealed record UpdateProductPrice(string IdOrHandle, decimal Price) : IRequest<Result>
{
    public sealed class Handler(IPublishEndpoint publishEndpoint, CatalogContext catalogContext = default!) : IRequestHandler<UpdateProductPrice, Result>
    {
        public async Task<Result> Handle(UpdateProductPrice request, CancellationToken cancellationToken)
        {
            var isId = int.TryParse(request.IdOrHandle, out var id);

            var product = isId ?
                await catalogContext.Products.FirstOrDefaultAsync(product => product.Id == id, cancellationToken)
                : await catalogContext.Products.FirstOrDefaultAsync(product => product.Handle == request.IdOrHandle, cancellationToken);

            if (product is null)
            {
                return Result.Failure(Errors.ProductNotFound);
            }

            product.Price = request.Price;

            await catalogContext.SaveChangesAsync(cancellationToken);

            await publishEndpoint.Publish(new Catalog.Contracts.ProductPriceUpdated
            {
                ProductId = product.Id,
                NewPrice = product.Price
            });

            return Result.Success();
        }
    }
}

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

            string cdnBaseUrl = configuration["CdnBaseUrl"] ?? "https://yourbrandstorage.blob.core.windows.net";

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

public sealed record UpdateProductHandle(string IdOrHandle, string Handle) : IRequest<Result>
{
    public sealed class Handler(IPublishEndpoint publishEndpoint, CatalogContext catalogContext = default!) : IRequestHandler<UpdateProductHandle, Result>
    {
        public async Task<Result> Handle(UpdateProductHandle request, CancellationToken cancellationToken)
        {
            var isId = int.TryParse(request.IdOrHandle, out var id);

            var product = isId ?
                await catalogContext.Products.FirstOrDefaultAsync(product => product.Id == id, cancellationToken)
                : await catalogContext.Products.FirstOrDefaultAsync(product => product.Handle == request.IdOrHandle, cancellationToken);

            if (product is null)
            {
                return Result.Failure(Errors.ProductNotFound);
            }

            var p = product;

            var handleInUse = await catalogContext.Products.AnyAsync(product => product.Id != p.Id && product.Handle == request.Handle, cancellationToken);

            if (handleInUse)
            {
                return Result.Failure(Errors.HandleAlreadyTaken);
            }

            product.Handle = request.Handle;

            await catalogContext.SaveChangesAsync(cancellationToken);

            await publishEndpoint.Publish(new Catalog.Contracts.ProductHandleUpdated
            {
                ProductId = product.Id,
                Handle = product.Handle
            });


            return Result.Success();
        }
    }
}

public sealed record UpdateProductCategory(string IdOrHandle, long ProductCategoryId) : IRequest<Result>
{
    public sealed class Handler(CatalogContext catalogContext = default!) : IRequestHandler<UpdateProductCategory, Result>
    {
        public async Task<Result> Handle(UpdateProductCategory request, CancellationToken cancellationToken)
        {
            var isId = int.TryParse(request.IdOrHandle, out var id);

            var product = isId ?
                await catalogContext.Products.FirstOrDefaultAsync(product => product.Id == id, cancellationToken)
                : await catalogContext.Products.FirstOrDefaultAsync(product => product.Handle == request.IdOrHandle, cancellationToken);

            if (product is null)
            {
                return Result.Failure(Errors.ProductNotFound);
            }

            var newCategory = await catalogContext.ProductCategories
            .Include(productCategory => productCategory.Parent)
             .Include(productCategory => productCategory.Products)
            .FirstOrDefaultAsync(productCategory => productCategory.Id == request.ProductCategoryId, cancellationToken);

            if (newCategory is null)
            {
                return Result.Failure(Errors.ProductNotFound);
            }

            product.Category.RemoveProduct(product);

            newCategory.AddProduct(product);

            await catalogContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}

public sealed record DeleteProduct(string IdOrHandle) : IRequest<Result>
{
    public sealed class Handler(CatalogContext catalogContext = default!) : IRequestHandler<DeleteProduct, Result>
    {
        public async Task<Result> Handle(DeleteProduct request, CancellationToken cancellationToken)
        {
            var isId = int.TryParse(request.IdOrHandle, out var id);

            var product = isId ?
                await catalogContext.Products.FirstOrDefaultAsync(product => product.Id == id, cancellationToken)
                : await catalogContext.Products.FirstOrDefaultAsync(product => product.Handle == request.IdOrHandle, cancellationToken);

            if (product is null)
            {
                return Result.Failure(Errors.ProductNotFound);
            }

            catalogContext.Products.Remove(product);

            await catalogContext.SaveChangesAsync(cancellationToken);


            return Result.Success();
        }
    }
}