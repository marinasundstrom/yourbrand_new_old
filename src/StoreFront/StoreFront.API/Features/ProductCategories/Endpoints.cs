using CatalogAPI;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace StoreFront.API.Features.ProductCategories;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapProductCategoriesEndpoints(this IEndpointRouteBuilder app)
    {
        var versionedApi = app.NewVersionedApi("ProductCategories");

        var productsGroup = versionedApi.MapGroup("/api/v{version:apiVersion}/productCategories")
            .WithTags("ProductCategories")
            .HasApiVersion(1, 0)
            .WithOpenApi()
            .RequireCors();

        productsGroup.MapGet("/", GetProductCategories)
            .WithName($"ProductCategories_{nameof(GetProductCategories)}");

        productsGroup.MapGet("{id}", GetProductCategoryById)
            .WithName($"ProductCategories_{nameof(GetProductCategoryById)}");

        return app;
    }

    private static async Task<Results<Ok<ProductCategoryTreeRootDto>, NotFound>> GetProductCategories(IProductCategoriesClient productCategoriesClient = default!, CancellationToken cancellationToken = default)
    {
        var tree = await productCategoriesClient.GetProductCategoryTreeAsync(cancellationToken);
        return tree is not null ? TypedResults.Ok(tree.ToProductCategoryTreeRootDto()) : TypedResults.NotFound();
    }

    private static async Task<Results<Ok<ProductCategoryDto>, NotFound>> GetProductCategoryById(string id, IProductCategoriesClient productCategoriesClient, CancellationToken cancellationToken = default)
    {
        var productCategory = await productCategoriesClient.GetProductCategoryByIdAsync(id, cancellationToken);
        return productCategory is not null ? TypedResults.Ok(productCategory.ToDto()) : TypedResults.NotFound();
    }
}

public record class ProductCategoryDto(long Id, string Name, string Description, string Handle, string Path, ProductCategoryParent? Parent, long ProductsCount);

public record class ProductCategoryTreeRootDto(IEnumerable<ProductCategoryTreeNodeDto> Categories, long ProductsCount);

public record class ProductCategoryTreeNodeDto(long Id, string Name, string Handle, string Path, string? Description, ProductCategoryParent? Parent, IEnumerable<ProductCategoryTreeNodeDto> SubCategories, long ProductsCount, bool CanAddProducts);

public record class ProductCategoryParent(long Id, string Name, string Handle, string Path, ProductCategoryParent? Parent, long ProductsCount);


public static class Mapping
{
    public static ProductCategoryDto ToDto(this CatalogAPI.ProductCategory productCategory)
    {
        return new(productCategory.Id, productCategory.Name, productCategory.Description ?? string.Empty, productCategory.Handle, productCategory.Path, null, productCategory.ProductsCount);
    }

    public static ProductCategoryTreeRootDto ToProductCategoryTreeRootDto(this CatalogAPI.ProductCategoryTreeRoot productCategory)
    {
        return new ProductCategoryTreeRootDto(productCategory.Categories.Select(x => x.ToProductCategoryTreeNodeDto()), productCategory.ProductsCount);
    }

    public static ProductCategoryTreeNodeDto ToProductCategoryTreeNodeDto(this CatalogAPI.ProductCategoryTreeNode productCategory)
    {
        return new(productCategory.Id, productCategory.Name, productCategory.Handle, productCategory.Path, productCategory.Description, productCategory.Parent?.ToParentDto(), productCategory.SubCategories.Select(x => x.ToProductCategoryTreeNodeDto()), productCategory.ProductsCount, productCategory.CanAddProducts);
    }

    public static ProductCategoryParent ToParentDto(this CatalogAPI.ParentProductCategoryTreeNode productCategory)
    {
        return new(productCategory.Id, productCategory.Name, productCategory.Handle, productCategory.Path, productCategory.Parent?.ToParentDto(), productCategory.ProductsCount);
    }

    public static ProductCategoryParent ToParentDto2(this CatalogAPI.ParentProductCategory productCategory)
    {
        return new(productCategory.Id, productCategory.Name, productCategory.Handle, productCategory.Path, productCategory.Parent?.ToParentDto2(), productCategory.ProductsCount);
    }

    public static ProductCategoryParent ToParentDto3(this CatalogAPI.ProductCategory2 productCategory)
    {
        return new(productCategory.Id, productCategory.Name, productCategory.Handle, productCategory.Path, productCategory.Parent?.ToParentDto2(), productCategory.ProductsCount);
    }
}