using CartsAPI;

using MassTransit;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace StoreFront.API.Features.Cart;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapCartEndpoints(this IEndpointRouteBuilder app)
    {
        string GetCartsExpire20 = nameof(GetCartsExpire20);

        var versionedApi = app.NewVersionedApi("Cart");

        var cartGroup = versionedApi.MapGroup("/api/v{version:apiVersion}/cart")
            .WithTags("Cart")
            .HasApiVersion(1, 0)
            .WithOpenApi()
            .RequireCors();

        cartGroup.MapGet("/", GetCart)
            .WithName($"Cart_{nameof(GetCart)}");

        cartGroup.MapPost("/items", AddCartItem)
            .WithName($"Cart_{nameof(AddCartItem)}");

        cartGroup.MapPut("/items/{cartItemId}/quantity", UpdateCartItemQuantity)
            .WithName($"Cart_{nameof(UpdateCartItemQuantity)}");

        cartGroup.MapDelete("/items/{cartItemId}", RemoveCartItem)
            .WithName($"Cart_{nameof(RemoveCartItem)}");

        return app;
    }

    private static async Task<Results<Ok<Features.Cart.Cart>, NotFound>> GetCart(MassTransitCartsClient cartsClient, CancellationToken cancellationToken)
    {
        var cart = await cartsClient.GetCartById("test", cancellationToken);

        //var cart = await client.GetCartByIdAsync("test", cancellationToken);
        return cart is not null ? TypedResults.Ok(cart) : TypedResults.NotFound();
    }

    private static async Task<Results<Ok<CartItem>, NotFound>> AddCartItem(AddCartItemRequest request, MassTransitCartsClient cartsClient, CancellationToken cancellationToken)
    {
        var cartItem = await cartsClient.AddCartItem(
            "test", request.Name, request.Image, request.ProductId, request.ProductHandle, request.Description, request.Price, request.RegularPrice, request.Quantity, cancellationToken);

        return cartItem is not null ? TypedResults.Ok(cartItem) : TypedResults.NotFound();
    }

    private static async Task<Results<Ok<CartItem>, NotFound>> UpdateCartItemQuantity(string cartItemId, UpdateCartItemQuantityRequest request, MassTransitCartsClient cartsClient, CancellationToken cancellationToken)
    {
        if (request.Quantity <= 0) throw new ArgumentException("Invalid quantity", nameof(request));

        var cartItem = await cartsClient.UpdateCartItemQuantity("test", cartItemId, request.Quantity, cancellationToken);

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

public sealed record AddCartItemRequest(string Name, string? Image, long? ProductId, string? ProductHandle, string Description, decimal Price, decimal? RegularPrice, int Quantity);

public sealed record UpdateCartItemQuantityRequest(int Quantity);


public sealed class Cart(string id, string name, IEnumerable<CartItem> items)
{
    public string Id { get; set; } = id;

    public string Name { get; set; } = name;

    public IEnumerable<CartItem> Items { get; set; } = items;
}

public sealed class CartItem(string id, string name, string? image, long? productId, string? productHandle, string description, decimal price, decimal? regularPrice, int quantity)
{
    public string Id { get; set; } = id;

    public string Name { get; set; } = name;

    public string? Image { get; set; } = image;

    public long? ProductId { get; set; } = productId;

    public string? ProductHandle { get; set; } = productHandle;

    public string Description { get; set; } = description;

    public decimal Price { get; set; } = price;

    public decimal? RegularPrice { get; set; } = regularPrice;

    public int Quantity { get; set; } = quantity;

    public decimal Total => Price * Quantity;
}