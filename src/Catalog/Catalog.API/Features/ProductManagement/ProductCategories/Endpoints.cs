using Catalog.API.Model;

using MediatR;

using Microsoft.AspNetCore.Http.HttpResults;

using Asp.Versioning.Builder;

namespace Catalog.API.Features.ProductManagement.ProductCategories;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapProductCategoriesEndpoints(this IEndpointRouteBuilder app)
    {
        string GetProductCategoriesExpire20 = nameof(GetProductCategoriesExpire20);

        var versionedApi = app.NewVersionedApi("ProductCategories");

        var group = versionedApi.MapGroup("/v{version:apiVersion}/productCategories")
            .WithTags("ProductCategories")
            .RequireRateLimiting("fixed")
            .HasApiVersion(1, 0)
            .WithOpenApi();

        /*

        app.MapGet("/{idOrPath}", GetProductCategoryById)
            .WithName($"ProductCategories_{nameof(GetProductCategoryById)}")
            .WithTags("ProductCategories")
            .WithOpenApi();

        */

        group.MapGet("tree", GetProductCategoryTree)
            .WithName($"ProductCategories_{nameof(GetProductCategoryTree)}")
            .WithTags("ProductCategories")
            .WithOpenApi();

        group.MapPost("/", CreateProductCategory)
            .WithName($"ProductCategories_{nameof(CreateProductCategory)}")
            .WithTags("ProductCategories")
            .WithOpenApi();

        group.MapPut("{idOrPath}", UpdateProductCategoryDetails)
            .WithName($"ProductCategories_{nameof(UpdateProductCategoryDetails)}")
            .WithTags("ProductCategories")
            .WithOpenApi();

        group.MapDelete("{idOrPath}", DeleteProductCategory)
            .WithName($"ProductCategories_{nameof(DeleteProductCategory)}")
            .WithTags("ProductCategories")
            .WithOpenApi();

        return app;
    }

    private static async Task<Ok<PagedResult<ProductCategory>>> GetProductCategories(int page = 1, int pageSize = 10, string? searchTerm = null,
            IMediator mediator = default!, CancellationToken cancellationToken = default!)
    {
        var pagedResult = await mediator.Send(new GetProductCategories(page, pageSize, searchTerm), cancellationToken);
        return TypedResults.Ok(pagedResult);
    }

    /*

    private static async Task<Results<Ok<ProductCategory>, NotFound>> GetProductCategoryById(string idOrPath,
        IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetProductCategoryById(idOrPath), cancellationToken);

        return result.IsSuccess ? TypedResults.Ok(result.GetValue()) : TypedResults.NotFound();
    }

    */

    private static async Task<Results<Ok<ProductCategoryTreeRootDto>, BadRequest>> GetProductCategoryTree(
        IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetProductCategoryTree(), cancellationToken);

        return result.IsSuccess ? TypedResults.Ok(result.GetValue()) : TypedResults.BadRequest();
    }

    private static async Task<Results<Ok<ProductCategory>, BadRequest, ProblemHttpResult>> CreateProductCategory(CreateProductCategoryRequest request,
        IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateProductCategory(request.Name, request.Description, request.ParentCategoryId, request.Handle), cancellationToken);

        return result.IsSuccess ? TypedResults.Ok(result.GetValue()) : TypedResults.BadRequest();
    }

    private static async Task<Results<Ok, NotFound>> UpdateProductCategoryDetails(string idOrPath, UpdateProductCategoryDetailsRequest request,
        IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpdateProductCategoryDetails(idOrPath, request.Name, request.Description), cancellationToken);

        return result.IsSuccess ? TypedResults.Ok() : TypedResults.NotFound();
    }

    private static async Task<Results<Ok, NotFound>> DeleteProductCategory(string idOrPath,
        IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteProductCategory(idOrPath), cancellationToken);

        return result.IsSuccess ? TypedResults.Ok() : TypedResults.NotFound();
    }
}

public sealed record CreateProductCategoryRequest(string Name, string Description, long ParentCategoryId, string Handle);

public sealed record UpdateProductCategoryDetailsRequest(string Name, string Description);

public static class Mapping
{
    public static ProductCategory ToDto(this Domain.Entities.ProductCategory productCategory)
    {
        return new(productCategory.Id, productCategory.Name, productCategory.Description, productCategory.Parent?.ToParentProductCategory(), productCategory.CanAddProducts, productCategory.ProductsCount, productCategory.Handle, productCategory.Path);
    }

    public static ParentProductCategory ToParentProductCategory(this Domain.Entities.ProductCategory productCategory)
    {
        return new(productCategory.Id, productCategory.Name, productCategory.Description, productCategory.Handle, productCategory.Path, productCategory.Parent?.ToParentProductCategory(), productCategory.ProductsCount);
    }

    public static ProductCategory2 ToProductCategory2(this Domain.Entities.ProductCategory productCategory)
    {
        return new(productCategory.Id, productCategory.Name, productCategory.Description, productCategory.Handle, productCategory.Path, productCategory.Parent?.ToParentProductCategory(), productCategory.ProductsCount);
    }

    public static ProductCategoryTreeNodeDto ToProductCategoryTreeNodeDto(this Domain.Entities.ProductCategory productCategory)
    {
        return new(productCategory.Id, productCategory.Name, productCategory.Handle, productCategory.Path, productCategory.Description, productCategory.Parent?.ToParentProductCategoryTreeNodeDto(), productCategory.SubCategories.Select(x => x.ToProductCategoryTreeNodeDto()), productCategory.ProductsCount, productCategory.CanAddProducts);
    }

    public static ParentProductCategoryTreeNodeDto ToParentProductCategoryTreeNodeDto(this Domain.Entities.ProductCategory productCategory)
    {
        return new(productCategory.Id, productCategory.Name, productCategory.Handle, productCategory.Path, productCategory.Parent?.ToParentProductCategoryTreeNodeDto(), productCategory.ProductsCount);
    }
}

public sealed record ProductCategory(
    long Id,
    string Name,
    string? Description,
    ParentProductCategory? Parent,
    bool CanAddProducts,
    long ProductsCount,
    string Handle,
    string Path
);


public sealed record ParentProductCategory(
    long Id,
    string Name,
    string? Description,
    string Handle,
    string Path,
    ParentProductCategory? Parent,
    long ProductsCount
);

public sealed record ProductCategory2(
    long Id,
    string Name,
    string? Description,
    string Handle,
    string Path,
    ParentProductCategory? Parent,
    long ProductsCount
);

public record class ProductCategoryTreeRootDto(
    IEnumerable<ProductCategoryTreeNodeDto> Categories,
    long ProductsCount);

public record class ProductCategoryTreeNodeDto(
    long Id,
    string Name,
    string Handle,
    string Path,
    string? Description,
    ParentProductCategoryTreeNodeDto? Parent,
    IEnumerable<ProductCategoryTreeNodeDto> SubCategories,
    long ProductsCount,
    bool CanAddProducts);

public record class ParentProductCategoryTreeNodeDto(
    long Id,
    string Name,
    string Handle,
    string Path,
    ParentProductCategoryTreeNodeDto? Parent,
    long ProductsCount);