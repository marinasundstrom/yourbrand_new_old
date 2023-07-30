using Catalog.API.Data;
using Catalog.API.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

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

        return app;
    }

    private static async Task<Ok<PagedResult<Product>>> GetProducts(int page = 1, int pageSize = 10, string? searchTerm = null, CatalogContext catalogContext = default!, CancellationToken cancellationToken = default!)
    {
        var query = catalogContext.Products.AsQueryable();

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
        var product = await catalogContext.Products.FirstOrDefaultAsync(product => product.Id == id, cancellationToken);
        return product is not null ? TypedResults.Ok(product) : TypedResults.NotFound();
    }
}