namespace BlazorApp.Cart;

public interface ICartService
{
    Task InitializeAsync();
    
    Task<IEnumerable<CartItem>> GetCartItemsAsync(CancellationToken cancellationToken = default);

    Task AddCartItem(string name, string? image, string? productId, string description, decimal price, decimal? regularPrice, int quantity);

    Task UpdateCartItemQuantity(string cartItemId, int quantity);

    Task RemoveCartItem(string cartItemId);

    IReadOnlyCollection<CartItem> Items { get;}

    event EventHandler? CartUpdated;
}

public sealed class CartItem(string id, string name, string? image, string? productId, string description, decimal price, decimal? regularPrice, int quantity) {
    public string Id { get; set; } = id;

    public string Name { get; set; } = name;

    public string? Image { get; set; } = image;

    public string? ProductId { get; set; } = productId;
    
    public string Description { get; set; } = description;

    public decimal Price { get; set; } = price;

    public decimal? RegularPrice  { get; set; } = regularPrice;

    public int Quantity { get; set; } = quantity;

    public decimal Total => Price * Quantity;
}
