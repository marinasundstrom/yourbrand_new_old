using CartsAPI;

namespace BlazorApp.Cart;

public sealed class CartService(MassTransitCartsClient cartsClient) : ICartService
{
    private readonly List<CartItem> _items = new();

    public async Task InitializeAsync()
    {
        //_items.AddRange(await GetCartItemsAsync());

        CartUpdated?.Invoke(this, EventArgs.Empty);
    }

    public async Task<IEnumerable<CartItem>> GetCartItemsAsync(CancellationToken cancellationToken = default)
    {
        var cart = await cartsClient.GetCartById("test", cancellationToken);
        return cart.Items;
    }

    public async Task AddCartItem(string name, string? image, long? productId, string? productHandle, string description, decimal price, decimal? regularPrice, int quantity)
    {
        throw new NotImplementedException();
    }

    public Task UpdateCartItemQuantity(string cartItemId, int quantity)
    {
        throw new NotImplementedException();
    }

    public Task RemoveCartItem(string cartItemId)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<CartItem> Items => _items;

    public event EventHandler? CartUpdated;
}