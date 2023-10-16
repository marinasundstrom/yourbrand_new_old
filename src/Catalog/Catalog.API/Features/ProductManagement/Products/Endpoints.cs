using Catalog.API.Features.ProductManagement.Attributes;
using Catalog.API.Features.ProductManagement.Options;
using Catalog.API.Features.ProductManagement.ProductCategories;
using Catalog.API.Model;

using FluentValidation;

using MediatR;

using Microsoft.AspNetCore.Http.HttpResults;

namespace Catalog.API.Features.ProductManagement.Products;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapProductsEndpoints(this IEndpointRouteBuilder app)
    {
        string GetProductsExpire20 = nameof(GetProductsExpire20);

        var group = app.MapGroup("/api/products")
            .WithTags("Products")
            .RequireRateLimiting("fixed")
            .WithOpenApi();

        group.MapGet("/", GetProducts)
            .WithName($"Products_{nameof(GetProducts)}")
            .CacheOutput(GetProductsExpire20);

        group.MapGet("/{idOrHandle}", GetProductById)
            .WithName($"Products_{nameof(GetProductById)}");

        group.MapPost("/", CreateProduct)
            .AddEndpointFilter<ValidationFilter<CreateProductRequest>>()
            .WithName($"Products_{nameof(CreateProduct)}");

        group.MapPut("/{idOrHandle}", UpdateProductDetails)
            .AddEndpointFilter<ValidationFilter<UpdateProductDetailsRequest>>()
            .WithName($"Products_{nameof(UpdateProductDetails)}");

        group.MapPut("/{idOrHandle}/price", UpdateProductPrice)
            .WithName($"Products_{nameof(UpdateProductPrice)}");

        group.MapPost("/{idOrHandle}/image", UploadProductImage)
            .WithName($"Products_{nameof(UploadProductImage)}")
            .DisableAntiforgery();

        group.MapPut("/{idOrHandle}/handle", UpdateProductHandle)
            .AddEndpointFilter<ValidationFilter<UpdateProductHandleRequest>>()
            .WithName($"Products_{nameof(UpdateProductHandle)}");

        group.MapPut("/{idOrHandle}/category", UpdateProductCategory)
            .AddEndpointFilter<ValidationFilter<CreateProductCategoryRequest>>()
            .WithName($"Products_{nameof(UpdateProductCategory)}");

        group.MapDelete("/{idOrHandle}", DeleteProduct)
            .WithName($"Products_{nameof(DeleteProduct)}");

        return app;
    }

    private static async Task<Ok<PagedResult<ProductDto>>> GetProducts(int page = 1, int pageSize = 10, string? searchTerm = null, string? categoryPath = null,
        string? sortBy = null, SortDirection? sortDirection = null, IMediator mediator = default!, CancellationToken cancellationToken = default!)
    {
        var pagedResult = await mediator.Send(new GetProducts(page, pageSize, searchTerm, categoryPath, sortBy, sortDirection), cancellationToken);
        return TypedResults.Ok(pagedResult);
    }

    private static async Task<Results<Ok<ProductDto>, NotFound>> GetProductById(string idOrHandle,
        IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetProductById(idOrHandle), cancellationToken);

        return result.IsSuccess ? TypedResults.Ok(result.GetValue()) : TypedResults.NotFound();
    }

    private static async Task<Results<Ok<ProductDto>, BadRequest, ProblemHttpResult>> CreateProduct(CreateProductRequest request,
        IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateProduct(request.Name, request.Description, request.CategoryId, request.Price, request.Handle), cancellationToken);

        return result.IsSuccess ? TypedResults.Ok(result.GetValue()) : TypedResults.BadRequest();
    }

    private static async Task<Results<Ok, NotFound>> UpdateProductDetails(string idOrHandle, UpdateProductDetailsRequest request,
        IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpdateProductDetails(idOrHandle, request.Name, request.Description), cancellationToken);

        return result.IsSuccess ? TypedResults.Ok() : TypedResults.NotFound();
    }

    private static async Task<Results<Ok, NotFound>> UpdateProductPrice(string idOrHandle, UpdateProductPriceRequest request,
        IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpdateProductPrice(idOrHandle, request.Price), cancellationToken);

        return result.IsSuccess ? TypedResults.Ok() : TypedResults.NotFound();
    }

    private static async Task<Results<Ok<string>, NotFound>> UploadProductImage(string idOrHandle, IFormFile file,
        IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpdateProductImage(idOrHandle, file.OpenReadStream(), file.FileName, file.ContentType), cancellationToken);

        return result.IsSuccess ? TypedResults.Ok(result.GetValue()) : TypedResults.NotFound();
    }

    private static async Task<Results<Ok, NotFound, ProblemHttpResult>> UpdateProductHandle(string idOrHandle, UpdateProductHandleRequest request,
        IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpdateProductHandle(idOrHandle, request.Handle), cancellationToken);

        return result.IsSuccess ? TypedResults.Ok() : TypedResults.NotFound();
    }

    private static async Task<Results<Ok, NotFound, ProblemHttpResult>> UpdateProductCategory(string idOrHandle, UpdateProductCategoryRequest request,
        IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpdateProductCategory(idOrHandle, request.ProductCategoryId), cancellationToken);

        return result.IsSuccess ? TypedResults.Ok() : TypedResults.NotFound();
    }

    private static async Task<Results<Ok, NotFound>> DeleteProduct(string idOrHandle,
    IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteProduct(idOrHandle), cancellationToken);

        return result.IsSuccess ? TypedResults.Ok() : TypedResults.NotFound();
    }
}

public sealed record CreateProductRequest(string Name, string Description, long CategoryId, decimal Price, string Handle)
{
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductRequestValidator()
        {
            RuleFor(p => p.Name).MaximumLength(60).NotEmpty();
            RuleFor(p => p.Description).MaximumLength(255).NotEmpty();
            RuleFor(p => p.CategoryId).GreaterThan(0);
            RuleFor(p => p.Handle).MaximumLength(60).NotEmpty();
        }
    }
}

public sealed record UpdateProductDetailsRequest(string Name, string Description)
{
    public class UpdateProductDetailsRequestValidator : AbstractValidator<UpdateProductDetailsRequest>
    {
        public UpdateProductDetailsRequestValidator()
        {
            RuleFor(p => p.Name).MaximumLength(60).NotEmpty();
            RuleFor(p => p.Description).MaximumLength(255).NotEmpty();
        }
    }
}

public sealed record UpdateProductPriceRequest(decimal Price);

public sealed record UpdateProductHandleRequest(string Handle)
{
    public class CreateHandleRequestValidator : AbstractValidator<UpdateProductHandleRequest>
    {
        public CreateHandleRequestValidator()
        {
            RuleFor(p => p.Handle).MaximumLength(60).NotEmpty();
        }
    }
}

public sealed record UpdateProductCategoryRequest(long ProductCategoryId)
{
    public class UpdateProductCategoryRequestValidator : AbstractValidator<UpdateProductCategoryRequest>
    {
        public UpdateProductCategoryRequestValidator()
        {
            RuleFor(p => p.ProductCategoryId).GreaterThan(0);
        }
    }
}


public sealed record ProductDto(
    long Id,
    string Name,
    ProductCategoryParent? Category,
    string Description,
    decimal Price,
    decimal? RegularPrice,
    string? Image,
    string Handle
);

public record class ParentProductDto(
    long Id,
    string Name,
    ProductCategoryParent? Category,
    string Description,
    decimal Price,
    decimal? RegularPrice,
    string? Image,
    string Handle);

public record class ProductAttributeDto(
    AttributeDto Attribute, AttributeValueDto? Value, bool ForVariant, bool IsMainAttribute);

public record class ProductOptionDto(
    OptionDto Option, bool IsInherited);