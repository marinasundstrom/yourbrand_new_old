using Catalog.API.Data;
using Catalog.API.Model;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MassTransit;

namespace Catalog.API.ProductCategories;

public static class Endpoints 
{
    public static IEndpointRouteBuilder MapProductCategoriesEndpoints(this IEndpointRouteBuilder app) 
    {  
        string GetProductCategoriesExpire20 = nameof(GetProductCategoriesExpire20);
        
        app.MapGet("/api/productCategories", GetProductCategories)
            .WithName($"ProductCategories_{nameof(GetProductCategories)}")
            .WithTags("ProductCategories")
            .WithOpenApi()
            .CacheOutput(GetProductCategoriesExpire20);

        app.MapGet("/api/productCategories/tree", GetProductCategoryTree)
            .WithName($"ProductCategories_{nameof(GetProductCategoryTree)}")
            .WithTags("ProductCategories")
            .WithOpenApi();

        app.MapPost("/api/productCategories", CreateProductCategory)
            .WithName($"ProductCategories_{nameof(CreateProductCategory)}")
            .WithTags("ProductCategories")
            .WithOpenApi();

        app.MapPut("/api/productCategories/{idOrPath}", UpdateProductCategoryDetails)
            .WithName($"ProductCategories_{nameof(UpdateProductCategoryDetails)}")
            .WithTags("ProductCategories")
            .WithOpenApi();

        app.MapDelete("/api/productCategories/{idOrPath}", DeleteProductCategory)
            .WithName($"ProductCategories_{nameof(DeleteProductCategory)}")
            .WithTags("ProductCategories")
            .WithOpenApi();

        return app;
    }

    private static async Task<Ok<PagedResult<ProductCategory>>> GetProductCategories(int page = 1, int pageSize = 10, string? searchTerm = null, CatalogContext catalogContext = default!, CancellationToken cancellationToken = default!)
    {
        var query = catalogContext.ProductCategories.AsNoTracking().AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()!) || x.Description.ToLower().Contains(searchTerm.ToLower()!));
        }

        var total = await query.CountAsync(cancellationToken);

        var productCategories = await query.OrderBy(x => x.Name)
            .Skip(pageSize * (page - 1))
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var pagedResult = new PagedResult<ProductCategory>(productCategories, total);

        return TypedResults.Ok(pagedResult);
    }

    private static async Task<Results<Ok<ProductCategoryTreeRootDto>, BadRequest>> GetProductCategoryTree(CatalogContext catalogContext, CancellationToken cancellationToken)
    {
        var query = catalogContext.ProductCategories
            .Include(x => x.Parent)
            .ThenInclude(x => x!.Parent)
            .Include(x => x.SubCategories.OrderBy(x => x.Name))
            .Where(x => x.Parent == null)
            .OrderBy(x => x.Name)
            .AsSingleQuery()
            .AsNoTracking();

        var itemGroups = await query
            .ToArrayAsync(cancellationToken);
            
        var root = new ProductCategoryTreeRootDto(itemGroups.Select(x => x.ToProductCategoryTreeNodeDto()), itemGroups.Sum(x => x.ProductsCount));

        return TypedResults.Ok(root);
    }

    private static async Task<Results<Ok<ProductCategory>, BadRequest, ProblemHttpResult>> CreateProductCategory(CreateProductCategoryRequest request, CatalogContext catalogContext, CancellationToken cancellationToken)
    {
        var pathInUse = await catalogContext.ProductCategories.AnyAsync(productCategory => productCategory.Path == request.Path, cancellationToken);

        if(pathInUse) 
        {
            return TypedResults.Problem(
                statusCode: 400,
                detail: "The specified path is already assigned to another productCategory.", 
                title: "Path already in use");
        }

        var productCategory = new ProductCategory() {
            Name = request.Name,
            Path = request.Path
        };
        catalogContext.ProductCategories.Add(productCategory);
        await catalogContext.SaveChangesAsync(cancellationToken);
        return TypedResults.Ok(productCategory);
    }

    private static async Task<Results<Ok, NotFound>> UpdateProductCategoryDetails(string idOrPath, UpdateProductCategoryDetailsRequest request, IPublishEndpoint publishEndpoint, CatalogContext catalogContext, CancellationToken cancellationToken)
    {
        var isId = int.TryParse(idOrPath, out var id);

        var productCategory = isId ? 
            await catalogContext.ProductCategories.FirstOrDefaultAsync(productCategory => productCategory.Id == id, cancellationToken)
            : await catalogContext.ProductCategories.FirstOrDefaultAsync(productCategory => productCategory.Path == idOrPath, cancellationToken);

        if(productCategory is null) 
        {
            return TypedResults.NotFound();
        }

        productCategory.Name = request.Name;
        productCategory.Description = request.Description;
        
        await catalogContext.SaveChangesAsync(cancellationToken);

        return TypedResults.Ok();
    }

    private static async Task<Results<Ok, NotFound>> DeleteProductCategory(string idOrPath, CatalogContext catalogContext, CancellationToken cancellationToken)
    {
        var isId = int.TryParse(idOrPath, out var id);

        var productCategory = isId ? 
            await catalogContext.ProductCategories.FirstOrDefaultAsync(productCategory => productCategory.Id == id, cancellationToken)
            : await catalogContext.ProductCategories.FirstOrDefaultAsync(productCategory => productCategory.Path == idOrPath, cancellationToken);

        if(productCategory is null) 
        {
            return TypedResults.NotFound();
        }

        catalogContext.ProductCategories.Remove(productCategory);
        
        await catalogContext.SaveChangesAsync(cancellationToken);

        return TypedResults.Ok();
    }
}

public sealed record CreateProductCategoryRequest(string Name, string Description, decimal Price, string Path);

public sealed record UpdateProductCategoryDetailsRequest(string Name, string Description);

public static class Mapping
{
    public static ProductCategoryTreeNodeDto ToProductCategoryTreeNodeDto(this Model.ProductCategory productCategory)
    {
        return new (productCategory.Id, productCategory.Name, productCategory.Handle, productCategory.Path, productCategory.Description, productCategory.Parent?.ToParentDto(), productCategory.SubCategories.Select(x => x.ToProductCategoryTreeNodeDto()), productCategory.ProductsCount, productCategory.CanAddProducts);
    }

    public static ParentProductCategoryDto ToParentDto(this Model.ProductCategory productCategory)
    {
        return new(productCategory.Id, productCategory.Name, productCategory.Handle, productCategory.Path, productCategory.Parent?.ToParentDto(), productCategory.ProductsCount);
    }
}

public record class ProductCategoryTreeRootDto(IEnumerable<ProductCategoryTreeNodeDto> Categories, long ProductsCount);

public record class ProductCategoryTreeNodeDto(long Id, string Name, string Handle, string Path, string? Description, ParentProductCategoryDto? Parent, IEnumerable<ProductCategoryTreeNodeDto> SubCategories, long ProductsCount, bool CanAddProducts);

public record class ParentProductCategoryDto(long Id, string Name, string Handle, string Path, ParentProductCategoryDto? Parent, long ProductsCount);