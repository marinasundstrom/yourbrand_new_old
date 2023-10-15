namespace BlazorApp.Products;

using BlazorApp.ProductCategories;

public interface IProductsService
{
    Task<PagedResult<Product>> GetProducts(int? page = 1, int? pageSize = 10, string? searchTerm = null, string? categoryPath = null, CancellationToken cancellationToken = default);

    Task<Product> GetProductById(string productId, CancellationToken cancellationToken = default);
}

public sealed record PagedResult<T>(IEnumerable<T> Items, int Total);

public sealed record Product(long Id, string Name, ProductCategoryParent? Category, string? Image, string Description, decimal Price, decimal? RegularPrice, string Handle);