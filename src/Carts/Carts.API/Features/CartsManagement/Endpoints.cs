using Carts.API.Domain.Entities;
using Carts.API.Features.CartsManagement.Requests;

using MediatR;

using Microsoft.AspNetCore.Http.HttpResults;

namespace Carts.API.Features.CartsManagement;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapCartsEndpoints(this IEndpointRouteBuilder app)
    {
        string GetCartsExpire20 = nameof(GetCartsExpire20);

        app.MapGet("/api/carts", GetCarts)
            .WithName($"Carts_{nameof(GetCarts)}")
            .WithTags("Carts")
            .WithOpenApi()
            .CacheOutput(GetCartsExpire20);

        app.MapGet("/api/carts/{id}", GetCartById)
            .WithName($"Carts_{nameof(GetCartById)}")
            .WithTags("Carts")
            .WithOpenApi();

        app.MapPost("/api/carts", CreateCart)
            .WithName($"Carts_{nameof(CreateCart)}")
            .WithTags("Carts")
            .WithOpenApi();

        app.MapPost("/api/carts/{id}/items", AddCartItem)
            .WithName($"Carts_{nameof(AddCartItem)}")
            .WithTags("Carts")
            .WithOpenApi();

        app.MapGet("/api/carts/{cartId}/items/{id}", GetCartItemById)
            .WithName($"Carts_{nameof(GetCartItemById)}")
            .WithTags("Carts")
            .WithOpenApi();

        app.MapPut("/api/carts/{cartId}/items/{id}/quantity", UpdateCartItemQuantity)
            .WithName($"Carts_{nameof(UpdateCartItemQuantity)}")
            .WithTags("Carts")
            .WithOpenApi();

        app.MapDelete("/api/carts/{cartId}/items/{id}", RemoveCartItem)
            .WithName($"Carts_{nameof(RemoveCartItem)}")
            .WithTags("Carts")
            .WithOpenApi();

        return app;
    }

    private static async Task<Ok<PagedResult<Cart>>> GetCarts(int page = 1, int pageSize = 10, IMediator mediator = default!, CancellationToken cancellationToken = default!)
    {
        var result = await mediator.Send(new GetCarts(page, pageSize), cancellationToken);
        return TypedResults.Ok(result.GetValue());
    }

    private static async Task<Results<Ok<Cart>, NotFound>> GetCartById(string id, IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetCartById(id), cancellationToken);

        if (result.HasError(Errors.CartNotFound))
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(result.GetValue());
    }

    private static async Task<Results<Created<Cart>, NotFound>> CreateCart(CreateCartRequest request, IMediator mediator, LinkGenerator linkGenerator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateCart(request.Name), cancellationToken);

        if (result.HasError(Errors.CartNotFound))
        {
            return TypedResults.NotFound();
        }

        var cart = result.GetValue();

        var path = linkGenerator.GetPathByName(nameof(GetCartById), new { id = cart.Id });

        return TypedResults.Created(path, cart);
    }

    private static async Task<Results<Created<CartItem>, NotFound>> AddCartItem(string id, AddCartItemRequest cartItemRequest, IMediator mediator = default!, LinkGenerator linkGenerator = default!, CancellationToken cancellationToken = default!)
    {
        var result = await mediator.Send(new AddCartItem(id, cartItemRequest.Name, cartItemRequest.Image, cartItemRequest.ProductId, cartItemRequest.ProductHandle, cartItemRequest.Description, cartItemRequest.Price, cartItemRequest.RegularPrice, cartItemRequest.Quantity), cancellationToken);

        if (result.HasError(Errors.CartNotFound))
        {
            return TypedResults.NotFound();
        }

        var cartItem = result.GetValue();

        var path = linkGenerator.GetPathByName(nameof(GetCartItemById), new { id = cartItem.Id });

        return TypedResults.Created(path, cartItem);
    }

    private static async Task<Results<Ok<CartItem>, NotFound>> GetCartItemById(string cartId, string id, IMediator mediator = default!, CancellationToken cancellationToken = default!)
    {
        var result = await mediator.Send(new GetCartItemById(cartId, id), cancellationToken);

        if (result.HasError(Errors.CartNotFound))
        {
            return TypedResults.NotFound();
        }

        if (result.HasError(Errors.CartItemNotFound))
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(result.GetValue());
    }

    private static async Task<Results<Ok<CartItem>, NotFound>> UpdateCartItemQuantity(string cartId, string id, int quantity, IMediator mediator = default!, LinkGenerator linkGenerator = default!, CancellationToken cancellationToken = default!)
    {
        var result = await mediator.Send(new UpdateCartItemQuantity(cartId, id, quantity), cancellationToken);

        if (result.HasError(Errors.CartNotFound))
        {
            return TypedResults.NotFound();
        }

        if (result.HasError(Errors.CartItemNotFound))
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(result.GetValue());
    }

    private static async Task<Results<Ok, NotFound>> RemoveCartItem(string cartId, string id, IMediator mediator = default!, LinkGenerator linkGenerator = default!, CancellationToken cancellationToken = default!)
    {
        var result = await mediator.Send(new RemoveCartItem(cartId, id), cancellationToken);

        if (result.HasError(Errors.CartNotFound))
        {
            return TypedResults.NotFound();
        }

        if (result.HasError(Errors.CartItemNotFound))
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok();
    }
}

public sealed record CreateCartRequest(string Name);

public sealed record AddCartItemRequest(string Name, string? Image, long? ProductId, string? ProductHandle, string Description, decimal Price, decimal? RegularPrice, int Quantity);