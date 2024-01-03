namespace YourBrand.YourService.API.Common;

public sealed record PagedResult<T>(IEnumerable<T> Items, int Total);