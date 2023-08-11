using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.ProductCategories;

public static class Endpoints 
{
    public static IEndpointRouteBuilder MapProductCategoriesEndpoints(this IEndpointRouteBuilder app) 
    {  
        app.MapGet("/api/ProductCategories", GetProductCategories)
            .WithName($"ProductCategories_{nameof(GetProductCategories)}")
            .WithTags("ProductCategories")
            .RequireCors()
            .WithOpenApi();

        app.MapGet("/api/ProductCategories/{id}", GetProductCategoryById)
            .WithName($"ProductCategories_{nameof(GetProductCategoryById)}")
            .WithTags("ProductCategories")
            .RequireCors()
            .WithOpenApi();

        return app;
    }

    private static async Task<Results<Ok<ProductCategoryTreeRootDto>, NotFound>> GetProductCategories(IProductCategoryService productCategoryService = default!,CancellationToken cancellationToken = default)
    {
        var tree = await productCategoryService.GetProductCategories(cancellationToken);
        return tree is not null ? TypedResults.Ok(tree) : TypedResults.NotFound();
    }

    private static async Task<Results<Ok<ProductCategoryDto>, NotFound>> GetProductCategoryById(string id, IProductCategoryService productCategoryService = default!,CancellationToken cancellationToken = default)
    {
        var productCategory = await productCategoryService.GetProductCategoryById(id, cancellationToken);
        return productCategory is not null ? TypedResults.Ok(productCategory) : TypedResults.NotFound();
    }
}