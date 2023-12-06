using StoreFrontAPI;

namespace BlazorApp.Cart;

public sealed class CartService(StoreFrontAPI.ICartClient client) : ICartService
{
    private readonly List<BlazorApp.Cart.CartItem> _items = new();

    public async Task InitializeAsync()
    {
        _items.Clear();
        _items.AddRange(await GetCartItemsAsync());

        CartUpdated?.Invoke(this, EventArgs.Empty);
    }

    public async Task<IEnumerable<BlazorApp.Cart.CartItem>> GetCartItemsAsync(CancellationToken cancellationToken = default)
    {
        var cart = await client.GetCartAsync(cancellationToken);
        return cart.Items!.Select(x => new BlazorApp.Cart.CartItem(x.Id!, x.Name!, x.Image!, x.ProductId, x.ProductHandle, x.Description!, (decimal)x.Price, (decimal?)x.RegularPrice, (int)x.Quantity));
    }

    public async Task AddCartItem(string name, string? image, long? productId, string? productHandle, string description, decimal price, decimal? regularPrice, int quantity)
    {
        var ci = await client.AddCartItemAsync(new AddCartItemRequest
        {
            Name = name,
            Image = image,
            ProductId = productId,
            ProductHandle = productHandle,
            Description = description,
            Price = price,
            RegularPrice = regularPrice,
            Quantity = quantity
        });

        var cartItem = _items.FirstOrDefault(x => x.ProductId == productId);

        if (cartItem is not null)
        {
            cartItem.Quantity += quantity;
        }
        else
        {
            cartItem = new BlazorApp.Cart.CartItem(ci.Id, name, image, productId, productHandle, description, price, regularPrice, quantity);

            _items.Add(cartItem);
        }

        CartUpdated?.Invoke(this, EventArgs.Empty);
    }

    public async Task UpdateCartItemQuantity(string cartItemId, int quantity)
    {
        await client.UpdateCartItemQuantityAsync(cartItemId, new UpdateCartItemQuantityRequest { Quantity = quantity });

        var cartItem = _items.FirstOrDefault(x => x.Id == cartItemId);

        if (cartItem is not null)
        {
            cartItem.Quantity = quantity;
        }

        CartUpdated?.Invoke(this, EventArgs.Empty);
    }

    public async Task RemoveCartItem(string cartItemId)
    {
        await client.RemoveCartItemAsync(cartItemId);

        var cartItem = _items.FirstOrDefault(x => x.Id == cartItemId);

        if (cartItem is not null)
        {
            _items.Remove(cartItem);
        }

        Console.WriteLine(cartItem.Id);

        CartUpdated?.Invoke(this, EventArgs.Empty);
    }

    public IReadOnlyCollection<BlazorApp.Cart.CartItem> Items => _items;

    public event EventHandler? CartUpdated;
}