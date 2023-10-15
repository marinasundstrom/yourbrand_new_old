using MassTransit;

namespace BlazorApp.Cart;

public sealed class MassTransitCartsClient(
    IRequestClient<Carts.Contracts.GetCartById> getCartByIdClient,
    IRequestClient<Carts.Contracts.AddCartItem> addCartItemClient,
    IRequestClient<Carts.Contracts.UpdateCartItemQuantity> updateCartItemQuantityClient,
    IRequestClient<Carts.Contracts.RemoveCartItem> removeCartItemClient)
{
    public async Task<Cart> GetCartById(string cartId, CancellationToken cancellationToken = default)
    {
        var response = await getCartByIdClient.GetResponse<Carts.Contracts.GetCartByIdResponse>(
            new Carts.Contracts.GetCartById { Id = cartId }, cancellationToken);

        return response.Message.Cart.Map();
    }

    public async Task<CartItem> AddCartItem(string cartId, string name, string? image, long? productId, string productHandle, string description, decimal price, decimal? regularPrice, int quantity, CancellationToken cancellationToken = default)
    {
        var request2 = new Carts.Contracts.AddCartItem
        {
            CartId = cartId,
            Name = name,
            Image = image,
            ProductId = productId,
            ProductHandle = productHandle,
            Description = description,
            Price = price,
            RegularPrice = regularPrice,
            Quantity = quantity
        };

        var response = await addCartItemClient.GetResponse<Carts.Contracts.AddCartItemResponse>(request2, cancellationToken);
        return response.Message.CartItem.Map();
    }

    public async Task<CartItem> UpdateCartItemQuantity(string cartId, string cartItemId, int quantity, CancellationToken cancellationToken = default)
    {
        var request2 = new Carts.Contracts.UpdateCartItemQuantity
        {
            CartId = "test",
            CartItemId = cartItemId,
            Quantity = quantity
        };

        var response = await updateCartItemQuantityClient.GetResponse<Carts.Contracts.UpdateCartItemQuantityResponse>(request2, cancellationToken);
        return response.Message.CartItem.Map();
    }

    public async Task RemoveCartItem(string cartId, string cartItemId, CancellationToken cancellationToken = default)
    {
        var request2 = new Carts.Contracts.RemoveCartItem
        {
            CartId = "test",
            CartItemId = cartItemId
        };

        var response = await removeCartItemClient.GetResponse<Carts.Contracts.RemoveCartItemResponse>(request2, cancellationToken);
    }
}

public static class Mapper
{
    public static Cart Map(this Carts.Contracts.Cart cart)
        => new(cart.Id!, cart.Name!, cart.Items.Select(cartItem => cartItem.Map()));

    public static CartItem Map(this Carts.Contracts.CartItem cartItem)
        => new(cartItem.Id!, cartItem.Name!, cartItem.Image!, cartItem.ProductId, cartItem.ProductHandle, cartItem.Description!, cartItem.Price, cartItem.RegularPrice, (int)cartItem.Quantity);
}