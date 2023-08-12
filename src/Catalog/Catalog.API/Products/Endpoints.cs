using Catalog.API.Data;
using Catalog.API.Model;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using Catalog.API.ProductCategories;

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

        app.MapGet("/api/products/{idOrHandle}", GetProductById)
            .WithName($"Products_{nameof(GetProductById)}")
            .WithTags("Products")
            .WithOpenApi();

        app.MapPost("/api/products", CreateProduct)
            .WithName($"Products_{nameof(CreateProduct)}")
            .WithTags("Products")
            .WithOpenApi();

        app.MapPut("/api/products/{idOrHandle}", UpdateProductDetails)
            .WithName($"Products_{nameof(UpdateProductDetails)}")
            .WithTags("Products")
            .WithOpenApi();

        app.MapPut("/api/products/{idOrHandle}/price", UpdateProductPrice)
            .WithName($"Products_{nameof(UpdateProductPrice)}")
            .WithTags("Products")
            .WithOpenApi();

        app.MapDelete("/api/products/{idOrHandle}", DeleteProduct)
            .WithName($"Products_{nameof(DeleteProduct)}")
            .WithTags("Products")
            .WithOpenApi();

        app.MapPost("/api/products/{idOrHandle}/image", UploadProductImage)
            .WithName($"Products_{nameof(UploadProductImage)}")
            .WithTags("Products")
            .WithOpenApi()
            .DisableAntiforgery();

        app.MapPut("/api/products/{idOrHandle}/handle", UpdateProductHandle)
            .WithName($"Products_{nameof(UpdateProductHandle)}")
            .WithTags("Products")
            .WithOpenApi();

        app.MapPut("/api/products/{idOrHandle}/category", UpdateProductCategory)
            .WithName($"Products_{nameof(UpdateProductCategory)}")
            .WithTags("Products")
            .WithOpenApi();

        return app;
    }

    private static async Task<Ok<PagedResult<Product>>> GetProducts(int page = 1, int pageSize = 10, string? searchTerm = null, string? categoryPath = null, CatalogContext catalogContext = default!, CancellationToken cancellationToken = default!)
    {
        var query = catalogContext.Products
            .Include(x => x.Category)
            .ThenInclude(x => x.Parent)
            .AsNoTracking().AsQueryable();

        if(!string.IsNullOrEmpty(categoryPath)) 
        {
            query = query.Where(x => x.Category.Path.StartsWith(categoryPath));
        }

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()!) || x.Description.ToLower().Contains(searchTerm.ToLower()!));
        }

        var total = await query.CountAsync(cancellationToken);

        var products = await query.OrderBy(x => x.Name)
            .Skip(pageSize * (page - 1))
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var pagedResult = new PagedResult<Product>(products.Select(x => x.ToDto()), total);

        return TypedResults.Ok(pagedResult);
    }

    private static async Task<Results<Ok<Product>, NotFound>> GetProductById(string idOrHandle, CatalogContext catalogContext, CancellationToken cancellationToken)
    {
        var isId = int.TryParse(idOrHandle, out var id);

        var query = catalogContext.Products
            .Include(x => x.Category)
            .ThenInclude(x => x.Parent)
            .AsQueryable();

        var product = isId ? 
            await query.FirstOrDefaultAsync(product => product.Id == id, cancellationToken)
            : await query.FirstOrDefaultAsync(product => product.Handle == idOrHandle, cancellationToken);

        return product is not null ? TypedResults.Ok(product.ToDto()) : TypedResults.NotFound();
    }

    private static async Task<Results<Ok<Product>, BadRequest, ProblemHttpResult>> CreateProduct(CreateProductRequest request, CatalogContext catalogContext, CancellationToken cancellationToken)
    {
        var handleInUse = await catalogContext.Products.AnyAsync(product => product.Handle == request.Handle, cancellationToken);

        if(handleInUse) 
        {
            return TypedResults.Problem(
                statusCode: 400,
                detail: "The specified handle is already assigned to another product.", 
                title: "Handle already in use");
        }

        var product = new Model.Product() {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Handle = request.Handle
        };

        catalogContext.Products.Add(product);

        await catalogContext.SaveChangesAsync(cancellationToken);

        return TypedResults.Ok(product.ToDto());
    }

    private static async Task<Results<Ok, NotFound>> UpdateProductDetails(string idOrHandle, UpdateProductDetailsRequest request, IPublishEndpoint publishEndpoint, CatalogContext catalogContext, CancellationToken cancellationToken)
    {
        var isId = int.TryParse(idOrHandle, out var id);

        var product = isId ? 
            await catalogContext.Products.FirstOrDefaultAsync(product => product.Id == id, cancellationToken)
            : await catalogContext.Products.FirstOrDefaultAsync(product => product.Handle == idOrHandle, cancellationToken);

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

    private static async Task<Results<Ok, NotFound>> UpdateProductPrice(string idOrHandle, UpdateProductPriceRequest request, IPublishEndpoint publishEndpoint, CatalogContext catalogContext, CancellationToken cancellationToken)
    {
        var isId = int.TryParse(idOrHandle, out var id);

        var product = isId ? 
            await catalogContext.Products.FirstOrDefaultAsync(product => product.Id == id, cancellationToken)
            : await catalogContext.Products.FirstOrDefaultAsync(product => product.Handle == idOrHandle, cancellationToken);

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

    private static async Task<Results<Ok, NotFound>> DeleteProduct(string idOrHandle, CatalogContext catalogContext, CancellationToken cancellationToken)
    {
        var isId = int.TryParse(idOrHandle, out var id);

        var product = isId ? 
            await catalogContext.Products.FirstOrDefaultAsync(product => product.Id == id, cancellationToken)
            : await catalogContext.Products.FirstOrDefaultAsync(product => product.Handle == idOrHandle, cancellationToken);

        if(product is null) 
        {
            return TypedResults.NotFound();
        }

        catalogContext.Products.Remove(product);
        
        await catalogContext.SaveChangesAsync(cancellationToken);

        return TypedResults.Ok();
    }

    private static async Task<Results<Ok<string>, NotFound>> UploadProductImage(string idOrHandle, IFormFile file, 
        BlobServiceClient blobServiceClient, IConfiguration configuration, IPublishEndpoint publishEndpoint, CatalogContext catalogContext, CancellationToken cancellationToken)
    {
        var isId = int.TryParse(idOrHandle, out var id);

        var product = isId ? 
            await catalogContext.Products.FirstOrDefaultAsync(product => product.Id == id, cancellationToken)
            : await catalogContext.Products.FirstOrDefaultAsync(product => product.Handle == idOrHandle, cancellationToken);

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

        await publishEndpoint.Publish(new Catalog.Contracts.ProductImageUpdated {
            ProductId = product.Id,
            ImageUrl = product.Image
        });

        return TypedResults.Ok(product.Image);
    }

    private static async Task<Results<Ok, NotFound, ProblemHttpResult>> UpdateProductHandle(string idOrHandle, UpdateProductHandleRequest request, IPublishEndpoint publishEndpoint, CatalogContext catalogContext, CancellationToken cancellationToken)
    {
        if(string.IsNullOrEmpty(request.Handle)) 
        {
            return TypedResults.Problem(
                statusCode: 400,
                detail: "The handle is empty or null.", 
                title: "Handle is null or empty");       
        }

        var isId = int.TryParse(idOrHandle, out var id);

        var product = isId ? 
            await catalogContext.Products.FirstOrDefaultAsync(product => product.Id == id, cancellationToken)
            : await catalogContext.Products.FirstOrDefaultAsync(product => product.Handle == idOrHandle, cancellationToken);

        if(product is null) 
        {
            return TypedResults.NotFound();
        }

        var p = product;

        var handleInUse = await catalogContext.Products.AnyAsync(product => product.Id != p.Id && product.Handle == request.Handle, cancellationToken);

        if(handleInUse) 
        {
            return TypedResults.Problem(
                statusCode: 400,
                detail: "The specified handle is already assigned to another product.", 
                title: "Handle already in use");
        }

        product.Handle = request.Handle;
        
        await catalogContext.SaveChangesAsync(cancellationToken);

        await publishEndpoint.Publish(new Catalog.Contracts.ProductHandleUpdated {
            ProductId = product.Id,
            Handle = product.Handle
        });

        return TypedResults.Ok();
    }

    private static async Task<Results<Ok, NotFound, ProblemHttpResult>> UpdateProductCategory(string idOrHandle, UpdateProductCategoryRequest request, IPublishEndpoint publishEndpoint, CatalogContext catalogContext, CancellationToken cancellationToken)
    {
        var isId = int.TryParse(idOrHandle, out var id);

        var query = catalogContext.Products
            .Include(product => product.Category)
            .ThenInclude(productCategory => productCategory.Parent)
            .AsQueryable();

        var product = isId ? 
            await query.FirstOrDefaultAsync(product => product.Id == id, cancellationToken)
            : await query.FirstOrDefaultAsync(product => product.Handle == idOrHandle, cancellationToken);

        if(product is null) 
        {
            return TypedResults.NotFound();
        }

        var newCategory = await catalogContext.ProductCategories
            .Include(productCategory => productCategory.Parent)
             .Include(productCategory => productCategory.Products)
            .FirstOrDefaultAsync(productCategory => productCategory.Id == request.ProductCategoryId, cancellationToken);

        if(newCategory is null) 
        {
            return TypedResults.NotFound();    
        }

        product.Category.RemoveProduct(product);
        
        newCategory.AddProduct(product);

        await catalogContext.SaveChangesAsync(cancellationToken);

        return TypedResults.Ok();
    }
}

public sealed record CreateProductRequest(string Name, string Description, decimal Price, string Handle);

public sealed record UpdateProductDetailsRequest(string Name, string Description);

public sealed record UpdateProductPriceRequest(decimal Price);

public sealed record UpdateProductHandleRequest(string Handle);

public sealed record UpdateProductCategoryRequest(long ProductCategoryId);


public sealed record Product(
    long Id, 
    string Name,
    ProductCategoryParent? Category, 
    string Description,
    decimal Price,
    decimal? RegularPrice,
    string? Image,
    string Handle
);

public static class Mapping
{
    public static Product ToDto(this Model.Product product)
    {
        return new(product.Id, product.Name, product.Category.ToShortDto(), product.Description, product.Price, product.RegularPrice, product.Image, product.Handle);
    }
}
