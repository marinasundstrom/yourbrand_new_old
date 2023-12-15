using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog;

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

        productsGroup.MapGet("{*id}", GetProductCategoryById)
            .WithName($"ProductCategories_{nameof(GetProductCategoryById)}");

        productsGroup.MapGet("tree/{*idOrPath}", GetProductCategoryTree)
            .WithName($"ProductCategories_{nameof(GetProductCategoryTree)}");

        return app;
    }

    private static async Task<Results<Ok<PagedResult<ProductCategoryDto>>, NotFound>> GetProductCategories(int? page = 1, int? pageSize = 10, string? searchTerm = null, IProductCategoriesClient productCategoriesClient = default!, CancellationToken cancellationToken = default)
    {
        var results = await productCategoriesClient.GetProductCategoriesAsync(null, null, false, false, page, pageSize, searchTerm, null, null, cancellationToken);
        return results is not null ? TypedResults.Ok(
                new PagedResult<ProductCategoryDto>(results.Items.Select(x => x.ToDto()), results.Total)
        ) : TypedResults.NotFound();
    }

    private static async Task<Results<Ok<ProductCategoryDto>, NotFound>> GetProductCategoryById(string id, IProductCategoriesClient productCategoriesClient, CancellationToken cancellationToken = default)
    {
        var productCategory = await productCategoriesClient.GetProductCategoryByIdAsync(id, cancellationToken);
        return productCategory is not null ? TypedResults.Ok(productCategory.ToDto()) : TypedResults.NotFound();
    }

    private static async Task<Results<Ok<ProductCategoryTreeRootDto>, NotFound>> GetProductCategoryTree(string? rootNodeIdOrPath, IProductCategoriesClient productCategoriesClient = default!, CancellationToken cancellationToken = default)
    {
        var tree = await productCategoriesClient.GetProductCategoryTreeAsync(null, rootNodeIdOrPath, cancellationToken);
        return tree is not null ? TypedResults.Ok(tree.ToProductCategoryTreeRootDto()) : TypedResults.NotFound();
    }
}

public record class ProductCategoryDto(long Id, string Name, string Description, string Handle, string Path, ProductCategoryParent? Parent, long ProductsCount);

public record class ProductCategoryTreeRootDto(IEnumerable<ProductCategoryTreeNodeDto> Categories, long ProductsCount);

public record class ProductCategoryTreeNodeDto(long Id, string Name, string Handle, string Path, string? Description, ProductCategoryParent? Parent, IEnumerable<ProductCategoryTreeNodeDto> SubCategories, long ProductsCount, bool CanAddProducts);

public record class ProductCategoryParent(long Id, string Name, string Handle, string Path, ProductCategoryParent? Parent, long ProductsCount);


public static class Mapping
{
    public static ProductCategoryDto ToDto(this YourBrand.Catalog.ProductCategory productCategory)
    {
        return new(productCategory.Id, productCategory.Name, productCategory.Description ?? string.Empty, productCategory.Handle, productCategory.Path, null, productCategory.ProductsCount);
    }

    public static ProductCategoryTreeRootDto ToProductCategoryTreeRootDto(this YourBrand.Catalog.ProductCategoryTreeRoot productCategory)
    {
        return new ProductCategoryTreeRootDto(productCategory.Categories.Select(x => x.ToProductCategoryTreeNodeDto()), productCategory.ProductsCount);
    }

    public static ProductCategoryTreeNodeDto ToProductCategoryTreeNodeDto(this YourBrand.Catalog.ProductCategoryTreeNode productCategory)
    {
        return new(productCategory.Id, productCategory.Name, productCategory.Handle, productCategory.Path, productCategory.Description, productCategory.Parent?.ToParentDto(), productCategory.SubCategories.Select(x => x.ToProductCategoryTreeNodeDto()), productCategory.ProductsCount, productCategory.CanAddProducts);
    }

    public static ProductCategoryParent ToParentDto(this YourBrand.Catalog.ParentProductCategoryTreeNode productCategory)
    {
        return new(productCategory.Id, productCategory.Name, productCategory.Handle, productCategory.Path, productCategory.Parent?.ToParentDto(), productCategory.ProductsCount);
    }

    public static ProductCategoryParent ToParentDto2(this YourBrand.Catalog.ParentProductCategory productCategory)
    {
        return new(productCategory.Id, productCategory.Name, productCategory.Handle, productCategory.Path, productCategory.Parent?.ToParentDto2(), productCategory.ProductsCount);
    }

    public static ProductCategoryParent ToParentDto3(this YourBrand.Catalog.ProductCategory2 productCategory)
    {
        return new(productCategory.Id, productCategory.Name, productCategory.Handle, productCategory.Path, productCategory.Parent?.ToParentDto2(), productCategory.ProductsCount);
    }
}