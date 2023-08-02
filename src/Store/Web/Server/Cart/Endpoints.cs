using CartsAPI;
using MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Cart;

public static class Endpoints 
{
    public static IEndpointRouteBuilder MapCartsEndpoints(this IEndpointRouteBuilder app) 
    {  
        string GetCartsExpire20 = nameof(GetCartsExpire20);

        app.MapGet("/api/cart", GetCart)
            .WithName($"Cart_{nameof(GetCart)}")
            .WithTags("Cart")
            .WithOpenApi();

        app.MapPost("/api/cart/items", AddCartItem)
            .WithName($"Cart_{nameof(AddCartItem)}")
            .WithTags("Cart")
            .WithOpenApi();

        app.MapPut("/api/cart/items/{cartItemId}/quantity", UpdateCartItemQuantity)
            .WithName($"Cart_{nameof(UpdateCartItemQuantity)}")
            .WithTags("Cart")
            .WithOpenApi();

        app.MapDelete("/api/cart/items/{cartItemId}", RemoveCartItem)
            .WithName($"Cart_{nameof(RemoveCartItem)}")
            .WithTags("Cart")
            .WithOpenApi();

        return app;
    }

    private static async Task<Results<Ok<Carts.Contracts.Cart>, NotFound>> GetCart(IRequestClient<Carts.Contracts.GetCartById> requestClient, CancellationToken cancellationToken)
    {
        var response = await requestClient.GetResponse<Carts.Contracts.GetCartByIdResponse>(
            new Carts.Contracts.GetCartById { Id = "test" }, cancellationToken);

        var cart = response.Message.Cart;

        //var cart = await client.GetCartByIdAsync("test", cancellationToken);
        return cart is not null ? TypedResults.Ok(cart) : TypedResults.NotFound();
    }

    private static async Task<Results<Ok<Carts.Contracts.CartItem>, NotFound>> AddCartItem(AddCartItemRequest request, IRequestClient<Carts.Contracts.AddCartItem> requestClient, CancellationToken cancellationToken)
    {
        var request2 = new Carts.Contracts.AddCartItem {
            CartId = "test",
            Name = request.Name,
            Image = request.Image,
            ProductId = request.ProductId,
            Description = request.Description,
            Price = request.Price,
            RegularPrice = request.RegularPrice,
            Quantity = request.Quantity
        };

        var response = await requestClient.GetResponse<Carts.Contracts.AddCartItemResponse>(request2, cancellationToken);
        var cartItem = response.Message.CartItem;

        return cartItem is not null ? TypedResults.Ok(cartItem) : TypedResults.NotFound();
    }

    private static async Task<Results<Ok<Carts.Contracts.CartItem>, NotFound>> UpdateCartItemQuantity(string cartItemId, int quantity, IRequestClient<Carts.Contracts.UpdateCartItemQuantity> requestClient, CancellationToken cancellationToken)
    {
        if(quantity <= 0) throw new ArgumentException("Invalid quantity", nameof(quantity));

        var request2 = new Carts.Contracts.UpdateCartItemQuantity {
            CartId = "test",
            CartItemId = cartItemId,
            Quantity = quantity
        };

        var response = await requestClient.GetResponse<Carts.Contracts.UpdateCartItemQuantityResponse>(request2, cancellationToken);
        var cartItem = response.Message.CartItem;

        //var cartItem = await client.UpdateCartItemQuantityAsync("test", cartItemId, quantity, cancellationToken);
        return cartItem is not null ? TypedResults.Ok(cartItem) : TypedResults.NotFound();
    }

    private static async Task<Results<Ok, NotFound>> RemoveCartItem(string cartItemId, IRequestClient<Carts.Contracts.RemoveCartItem> requestClient, CancellationToken cancellationToken)
    {

        var request2 = new Carts.Contracts.RemoveCartItem {
            CartId = "test",
            CartItemId = cartItemId
        };

        var response = await requestClient.GetResponse<Carts.Contracts.RemoveCartItemResponse>(request2, cancellationToken);

        //await client.RemoveCartItemAsync("test", cartItemId, cancellationToken);
        return TypedResults.Ok();
    }
}

public sealed record AddCartItemRequest(string Name, string? Image, string? ProductId, string Description, decimal Price, decimal? RegularPrice, int Quantity);
