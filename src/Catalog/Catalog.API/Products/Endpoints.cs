using Catalog.API.Data;
using Catalog.API.Model;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MassTransit;

namespace Catalog.API.Products;

public static class Endpoints 
{
    public static IEndpointRouteBuilder MapProductsEndpoints(this IEndpointRouteBuilder app) 
    {  
        string GetProductsExpire20 = nameof(GetProductsExpire20);

        app.MapGet("/api/products", GetProducts)
            .WithName($"Products_{nameof(GetProducts)}")
            .WithTags("Products")
            .WithOpenApi()
            .CacheOutput(GetProductsExpire20);

        app.MapGet("/api/products/{id}", GetProductById)
            .WithName($"Products_{nameof(GetProductById)}")
            .WithTags("Products")
            .WithOpenApi();

        app.MapPost("/api/products", CreateProduct)
            .WithName($"Products_{nameof(CreateProduct)}")
            .WithTags("Products")
            .WithOpenApi();

        app.MapPut("/api/products/{id}", UpdateProductDetails)
            .WithName($"Products_{nameof(UpdateProductDetails)}")
            .WithTags("Products")
            .WithOpenApi();

        app.MapPut("/api/products/{id}/price", UpdateProductPrice)
            .WithName($"Products_{nameof(UpdateProductPrice)}")
            .WithTags("Products")
            .WithOpenApi();

        app.MapDelete("/api/products/{id}", DeleteProduct)
            .WithName($"Products_{nameof(DeleteProduct)}")
            .WithTags("Products")
            .WithOpenApi();

        app.MapPost("/api/products/{id}/image", UploadProductImage)
            .WithName($"Products_{nameof(UploadProductImage)}")
            .WithTags("Products")
            .WithOpenApi()
            .DisableAntiforgery();
        return app;
    }

    private static async Task<Ok<PagedResult<Product>>> GetProducts(int page = 1, int pageSize = 10, string? searchTerm = null, CatalogContext catalogContext = default!, CancellationToken cancellationToken = default!)
    {
        var query = catalogContext.Products.AsNoTracking().AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()!) || x.Description.ToLower().Contains(searchTerm.ToLower()!));
        }

        var total = await query.CountAsync(cancellationToken);

        var products = await query.OrderBy(x => x.Name)
            .Skip(pageSize * (page - 1))
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var pagedResult = new PagedResult<Product>(products, total);

        return TypedResults.Ok(pagedResult);
    }

    private static async Task<Results<Ok<Product>, NotFound>> GetProductById(string id, CatalogContext catalogContext, CancellationToken cancellationToken)
    {
        var product = await catalogContext.Products.AsNoTracking().FirstOrDefaultAsync(product => product.Id == id, cancellationToken);
        return product is not null ? TypedResults.Ok(product) : TypedResults.NotFound();
    }

    private static async Task<Results<Ok<Product>, BadRequest>> CreateProduct(CreateProductRequest request, CatalogContext catalogContext, CancellationToken cancellationToken)
    {
        var product = new Product() {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price
        };
        catalogContext.Products.Add(product);
        await catalogContext.SaveChangesAsync(cancellationToken);
        return TypedResults.Ok(product);
    }

    private static async Task<Results<Ok, NotFound>> UpdateProductDetails(string id, UpdateProductDetailsRequest request, IPublishEndpoint publishEndpoint, CatalogContext catalogContext, CancellationToken cancellationToken)
    {
        var product = await catalogContext.Products.FirstOrDefaultAsync(product => product.Id == id, cancellationToken);
        
        if(product is null) 
        {
            return TypedResults.NotFound();
        }

        product.Name = request.Name;
        product.Description = request.Description;
        
        await catalogContext.SaveChangesAsync(cancellationToken);

        await publishEndpoint.Publish(new Catalog.Contracts.ProductDetailsUpdated {
            ProductId = product.Id,
            Name = product.Name,
            Description = product.Description
        });

        return TypedResults.Ok();
    }

    private static async Task<Results<Ok, NotFound>> UpdateProductPrice(string id, UpdateProductPriceRequest request, IPublishEndpoint publishEndpoint, CatalogContext catalogContext, CancellationToken cancellationToken)
    {
        var product = await catalogContext.Products.FirstOrDefaultAsync(product => product.Id == id, cancellationToken);
        
        if(product is null) 
        {
            return TypedResults.NotFound();
        }

        product.Price = request.Price;
        
        await catalogContext.SaveChangesAsync(cancellationToken);

        await publishEndpoint.Publish(new Catalog.Contracts.ProductPriceUpdated {
            ProductId = product.Id,
            NewPrice = product.Price
        });

        return TypedResults.Ok();
    }

    private static async Task<Results<Ok, NotFound>> DeleteProduct(string id, CatalogContext catalogContext, CancellationToken cancellationToken)
    {
        var product = await catalogContext.Products.FirstOrDefaultAsync(product => product.Id == id, cancellationToken);
        
        if(product is null) 
        {
            return TypedResults.NotFound();
        }

        catalogContext.Products.Remove(product);
        
        await catalogContext.SaveChangesAsync(cancellationToken);

        return TypedResults.Ok();
    }

    private static async Task<Results<Ok<string>, NotFound>> UploadProductImage(string id, IFormFile file, 
        BlobServiceClient blobServiceClient, IConfiguration configuration, CatalogContext catalogContext, CancellationToken cancellationToken)
    {
        var product = await catalogContext.Products.FirstOrDefaultAsync(product => product.Id == id, cancellationToken);
        
        if(product is null) 
        {
            return TypedResults.NotFound();
        }

        var blobContainerClient = blobServiceClient.GetBlobContainerClient("images");
        await blobContainerClient.CreateIfNotExistsAsync();

        BlobClient blobClient = blobContainerClient.GetBlobClient($"products/{file.FileName}");

        await blobClient.UploadAsync(file.OpenReadStream(), new BlobHttpHeaders { ContentType = file.ContentType });

        string cdnBaseUrl = configuration["CdnBaseUrl"] ?? "https://yourbrandstorage.blob.core.windows.net";

        product.Image = $"{cdnBaseUrl}/images/products/{file.FileName}";

        await catalogContext.SaveChangesAsync(cancellationToken);

        return TypedResults.Ok(product.Image);
    }
}

public sealed record CreateProductRequest(string Name, string Description, decimal Price);

public sealed record UpdateProductDetailsRequest(string Name, string Description);

public sealed record UpdateProductPriceRequest(decimal Price);