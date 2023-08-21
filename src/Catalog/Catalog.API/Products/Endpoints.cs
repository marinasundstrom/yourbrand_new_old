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
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;
using MassTransit.Transports;

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
            .RequireRateLimiting("fixed")
            .CacheOutput(GetProductsExpire20);

        app.MapGet("/api/products/{idOrHandle}", GetProductById)
            .WithName($"Products_{nameof(GetProductById)}")
            .WithTags("Products")
            .RequireRateLimiting("fixed")
            .WithOpenApi();

        app.MapPost("/api/products", CreateProduct)
            .AddEndpointFilter<ValidationFilter<CreateProductRequest>>()
            .WithName($"Products_{nameof(CreateProduct)}")
            .WithTags("Products")
            .RequireRateLimiting("fixed")
            .WithOpenApi();

        app.MapPut("/api/products/{idOrHandle}", UpdateProductDetails)
            .AddEndpointFilter<ValidationFilter<UpdateProductDetailsRequest>>()
            .WithName($"Products_{nameof(UpdateProductDetails)}")
            .WithTags("Products")
            .RequireRateLimiting("fixed")
            .WithOpenApi();

        app.MapPut("/api/products/{idOrHandle}/price", UpdateProductPrice)
            .WithName($"Products_{nameof(UpdateProductPrice)}")
            .WithTags("Products")
            .RequireRateLimiting("fixed")
            .WithOpenApi();

        app.MapPost("/api/products/{idOrHandle}/image", UploadProductImage)
            .WithName($"Products_{nameof(UploadProductImage)}")
            .WithTags("Products")
            .RequireRateLimiting("fixed")
            .WithOpenApi()
            .DisableAntiforgery();

        app.MapPut("/api/products/{idOrHandle}/handle", UpdateProductHandle)
            .AddEndpointFilter<ValidationFilter<UpdateProductHandleRequest>>()
            .WithName($"Products_{nameof(UpdateProductHandle)}")
            .WithTags("Products")
            .RequireRateLimiting("fixed")
            .WithOpenApi();

        app.MapPut("/api/products/{idOrHandle}/category", UpdateProductCategory)
            .AddEndpointFilter<ValidationFilter<CreateProductCategoryRequest>>()
            .WithName($"Products_{nameof(UpdateProductCategory)}")
            .WithTags("Products")
            .RequireRateLimiting("fixed")
            .WithOpenApi();

        app.MapDelete("/api/products/{idOrHandle}", DeleteProduct)
            .WithName($"Products_{nameof(DeleteProduct)}")
            .WithTags("Products")
            .RequireRateLimiting("fixed")
            .WithOpenApi();

        return app;
    }

    private static async Task<Ok<PagedResult<Product>>> GetProducts(int page = 1, int pageSize = 10, string? searchTerm = null, string? categoryPath = null,
        string? sortBy = null, SortDirection? sortDirection = null, IMediator mediator = default!, CancellationToken cancellationToken = default!)
    {
        var pagedResult = await mediator.Send(new GetProducts(page, pageSize, searchTerm, categoryPath, sortBy, sortDirection), cancellationToken);
        return TypedResults.Ok(pagedResult);
    }

    private static async Task<Results<Ok<Product>, NotFound>> GetProductById(string idOrHandle,
        IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetProductById(idOrHandle), cancellationToken);

        return result.IsSuccess ? TypedResults.Ok(result.GetValue()) : TypedResults.NotFound();
    }

    private static async Task<Results<Ok<Product>, BadRequest, ProblemHttpResult>> CreateProduct(CreateProductRequest request,
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
