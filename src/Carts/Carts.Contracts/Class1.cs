namespace Carts.Contracts;

public sealed record Cart(string Id, string Name, IEnumerable<CartItem> Items);

public sealed record CartItem(string Id, string Name, string Description, string? ProductId, decimal Price, decimal? RegularPrice, int Quantity);

public sealed record GetCarts(int Page = 1, int PageSize = 10);

public sealed record GetCartById(string CartId);

public sealed record AddCartItem(string CartId, string Name, string Description, string? ProductId, decimal Price, decimal? RegularPrice, int Quantity);

public sealed record RemoveCartItem(string CartItemId);

public sealed record UpdateCartItemQuantity(string CartItemId, int Quantity);

public sealed record CartItemAdded(string CartItemId);

public sealed record CartItemUpdated(string CartItemId);

public sealed record CartItemRemoved(string CartItemId);

public sealed record PagedResult<T>(IEnumerable<T> Items, int Total);