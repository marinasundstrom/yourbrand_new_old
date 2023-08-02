using CartsAPI;
using MassTransit;

namespace BlazorApp.Cart;

public sealed class CartService(ICartsClient client, IRequestClient<Carts.Contracts.GetCartById> requestClient) : ICartService
{
    private List<CartItem> _items = new();

    public async Task InitializeAsync() 
    {
        //_items.AddRange(await GetCartItemsAsync());

        CartUpdated?.Invoke(this, EventArgs.Empty);
    }

    public async Task<IEnumerable<CartItem>> GetCartItemsAsync(CancellationToken cancellationToken = default)
    {
        var response = await requestClient.GetResponse<Carts.Contracts.GetCartByIdResponse>(
            new Carts.Contracts.GetCartById { Id = "test" }, cancellationToken);

        return response.Message.Cart.Items!.Select(x => new CartItem(x.Id!, x.Name!, x.Image!, x.ProductId, x.Description!, x.Price, x.RegularPrice, (int)x.Quantity));
    }

    public async Task AddCartItem(string name, string? image, string? productId, string description, decimal price, decimal? regularPrice, int quantity)
    {

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