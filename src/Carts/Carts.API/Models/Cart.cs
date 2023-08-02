namespace Carts.API.Model;

public sealed class Cart 
{
    private HashSet<CartItem> _cartItems = new HashSet<CartItem>();

    public Cart(string name) 
    {
        Name = name;
    }

    public string Id { get; private set; } = Guid.NewGuid().ToString();

    public string Name { get; set; } = default!;

    public decimal Total { get; private set; }

    public IReadOnlyCollection<CartItem> Items => _cartItems;

    public CartItem AddItem(string name, string? image, string? productId, string description, decimal price, decimal? regularPrice, int quantity)
    {
        var cartItem = _cartItems.FirstOrDefault(item => item.ProductId == productId);

        if(cartItem is null)
        {
            cartItem = new CartItem(name, image, productId, description, price, regularPrice, quantity);
            _cartItems.Add(cartItem);
        }
        else 
        {
            cartItem.Quantity += quantity;
        }

        Total += price * quantity;

        return cartItem;
    }

    public void RemoveItem(string cartId)
    {
        var cartItem = _cartItems.FirstOrDefault(item => item.Id == cartId);

        if(cartItem is not null) 
        {
            _cartItems.Remove(cartItem);

            Total -= cartItem.Price * (decimal)cartItem.Quantity;
        }
    }
}

public sealed class CartItem 
{
    private CartItem()
    {

    }

    public CartItem(string name, string? image, string? productId, string description, decimal price, decimal? regularPrice, int quantity)
    {
        Name = name;
        Image = image;
        ProductId = productId;
        Description = description;
        Price = price;
        RegularPrice = regularPrice;
        Quantity = quantity;

        Created = DateTimeOffset.UtcNow;
    }

    public string Id { get; private set; } = Guid.NewGuid().ToString();

    public string Name { get; set; } = default!;

    public string? Image { get; set; }

    public string? ProductId { get; set; }

    public string Description { get; set; } = default!;

    public decimal Price { get; set; }

    public decimal? RegularPrice { get; set; }

    public double Quantity { get; set; }

    public decimal Total => Price * (decimal)Quantity;

    public DateTimeOffset Created { get; private set; }
}