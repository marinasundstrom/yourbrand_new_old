
namespace BlazorApp.Products;

public interface IProductsService
{
    Task<PagedResult<Product>> GetProducts(int? page = 1, int? pageSize = 10, string? searchTerm = null, string? categoryPath = null, CancellationToken cancellationToken = default);

    Task<Product> GetProductById(string productIdOrHandle, CancellationToken cancellationToken = default);

    Task<Product?> FindProductVariantByAttributes(string productIdOrHandle, Dictionary<string, string?> selectedAttributeValues, CancellationToken cancellationToken = default);

    Task<IEnumerable<Product>> FindProductVariantsByAttributes(string productIdOrHandle, Dictionary<string, string?> selectedAttributeValues, CancellationToken cancellationToken = default);

    Task<PagedResult<Product>> GetProductVariants(string productIdOrHandle, int page = 1, int pageSize = 10, string? searchString = null, CancellationToken cancellationToken = default);
}

public sealed record PagedResult<T>(IEnumerable<T> Items, int Total);