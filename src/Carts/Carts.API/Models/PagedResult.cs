namespace Carts.API.Model;

public sealed record PagedResult<T>(IEnumerable<T> Items, int Total);