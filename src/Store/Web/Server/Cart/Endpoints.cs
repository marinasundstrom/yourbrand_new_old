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

    private static async Task<Results<Ok<BlazorApp.Cart.Cart>, NotFound>> GetCart(MassTransitCartsClient cartsClient, CancellationToken cancellationToken)
    {
        var cart = await cartsClient.GetCartById("test", cancellationToken);

        //var cart = await client.GetCartByIdAsync("test", cancellationToken);
        return cart is not null ? TypedResults.Ok(cart) : TypedResults.NotFound();
    }

    private static async Task<Results<Ok<CartItem>, NotFound>> AddCartItem(AddCartItemRequest request, MassTransitCartsClient cartsClient, CancellationToken cancellationToken)
    {
        var cartItem = await cartsClient.AddCartItem(
            "test", request.Name, request.Image, request.ProductId, request.Description, request.Price, request.RegularPrice, request.Quantity, cancellationToken);

        return cartItem is not null ? TypedResults.Ok(cartItem) : TypedResults.NotFound();
    }

    private static async Task<Results<Ok<CartItem>, NotFound>> UpdateCartItemQuantity(string cartItemId, int quantity, MassTransitCartsClient cartsClient, CancellationToken cancellationToken)
    {
        if(quantity <= 0) throw new ArgumentException("Invalid quantity", nameof(quantity));

        var cartItem = await cartsClient.UpdateCartItemQuantity("test", cartItemId, quantity, cancellationToken);

        //var cartItem = await client.UpdateCartItemQuantityAsync("test", cartItemId, quantity, cancellationToken);
        return cartItem is not null ? TypedResults.Ok(cartItem) : TypedResults.NotFound();
    }

    private static async Task<Results<Ok, NotFound>> RemoveCartItem(string cartItemId, MassTransitCartsClient cartsClient, CancellationToken cancellationToken)
    {
        await cartsClient.RemoveCartItem("test", cartItemId, cancellationToken);

        //await client.RemoveCartItemAsync("test", cartItemId, cancellationToken);
        return TypedResults.Ok();
    }
}

public sealed record AddCartItemRequest(string Name, string? Image, string? ProductId, string Description, decimal Price, decimal? RegularPrice, int Quantity);
