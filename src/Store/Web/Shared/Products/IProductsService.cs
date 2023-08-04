namespace BlazorApp.Products;

public interface IProductsService
{
    Task<PagedResult<Product>> GetProducts(int? page = 1, int? pageSize = 10, string? searchTerm = null, CancellationToken cancellationToken = default);

    Task<Product> GetProductById(string productId, CancellationToken cancellationToken = default);
}

public sealed record PagedResult<T>(IEnumerable<T> Items, int Total);

public sealed record Product(string Id, string Name, string? Image, string Description, decimal Price, decimal? RegularPrice);