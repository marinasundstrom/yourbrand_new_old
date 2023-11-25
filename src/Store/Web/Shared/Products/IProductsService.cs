namespace BlazorApp.Products;

public interface IProductsService
{
    Task<PagedResult<Product>> GetProducts(int? page = 1, int? pageSize = 10, string? searchTerm = null, string? categoryPath = null, CancellationToken cancellationToken = default);

    Task<Product> GetProductById(string productId, CancellationToken cancellationToken = default);
}

public sealed record PagedResult<T>(IEnumerable<T> Items, int Total);