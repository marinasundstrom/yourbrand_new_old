namespace Carts.API.Domain.Entities;

public sealed class Cart
{
    private readonly HashSet<CartItem> _cartItems = new HashSet<CartItem>();

    public Cart(string tag)
    {
        Tag = tag;
    }

    internal Cart(string id, string tag)
    {
        Id = id;
        Tag = tag;
    }

    public string Id { get; private set; } = Guid.NewGuid().ToString();

    public string Tag { get; set; } = default!;

    public decimal Total { get; private set; }

    public IReadOnlyCollection<CartItem> Items => _cartItems;

    public CartItem AddItem(string name, string? image, long? productId, string? productHandle, string description, decimal price, decimal? regularPrice, int quantity, string? data)
    {
        var cartItem = _cartItems.FirstOrDefault(item => item.ProductId == productId && item.Data == data);

        if (cartItem is null)
        {
            cartItem = new CartItem(name, image, productId, productHandle, description, price, regularPrice, quantity, data);
            _cartItems.Add(cartItem);
            Total += cartItem.Total;
        }
        else
        {
            UpdateCartItemQuantity(cartItem.Id, (int)cartItem.Quantity + quantity);
        }

        return cartItem;
    }

    public void RemoveItem(string cartId)
    {
        var cartItem = _cartItems.FirstOrDefault(item => item.Id == cartId);

        if (cartItem is not null)
        {
            _cartItems.Remove(cartItem);

            Total -= cartItem.Total;
        }
    }

    public void UpdateCartItemQuantity(string cartId, int quantity)
    {
        var cartItem = _cartItems.FirstOrDefault(item => item.Id == cartId);

        if (cartItem is not null)
        {
            decimal oldTotal = cartItem.Total;
            Total -= oldTotal;

            cartItem.Quantity = quantity;
            Total += cartItem.Total;
        }
    }
}

public sealed class CartItem
{
    private CartItem()
    {

    }

    public CartItem(string name, string? image, long? productId, string? productHandle, string description, decimal price, decimal? regularPrice, int quantity, string? data)
    {
        Name = name;
        Image = image;
        ProductId = productId;
        ProductHandle = productHandle;
        Description = description;
        Price = price;
        RegularPrice = regularPrice;
        Quantity = quantity;
        Data = data;

        Created = DateTimeOffset.UtcNow;
    }

    public string Id { get; private set; } = Guid.NewGuid().ToString();

    public string Name { get; set; } = default!;

    public string? Image { get; set; }

    public long? ProductId { get; set; }

    public string? ProductHandle { get; set; }

    public string Description { get; set; } = default!;

    public decimal Price { get; set; }

    public decimal? RegularPrice { get; set; }

    public double Quantity { get; set; }

    public decimal Total => Price * (decimal)Quantity;

    public string? Data { get; private set; }

    public void UpdateData(string? data)
    {
        Data = data;
    }

    public DateTimeOffset Created { get; private set; }
}