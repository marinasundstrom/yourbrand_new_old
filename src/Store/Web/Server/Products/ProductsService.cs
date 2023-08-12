namespace BlazorApp.Products;

using CatalogAPI;
using BlazorApp.ProductCategories;

public sealed class ProductsService(IProductsClient productsClient) : IProductsService
{
    public async Task<PagedResult<Product>> GetProducts(int? page = 1, int? pageSize = 10, string? searchTerm = null, string? categoryPath = null, CancellationToken cancellationToken = default) 
    {
        var results = await productsClient.GetProductsAsync(page, pageSize, searchTerm, categoryPath, cancellationToken);
        return new PagedResult<Product>(results.Items.Select(product => product.Map()), results.Total);
    }

    public async Task<Product> GetProductById(string productId, CancellationToken cancellationToken = default) 
    {
        var product = await productsClient.GetProductByIdAsync(productId, cancellationToken);
        return product.Map();
    }
}

public static class Mapper 
{
    public static Product Map(this CatalogAPI.Product product) 
        => new (product.Id!, product.Name!, product?.Category.ToParentDto2(), product.Image!, product.Description!, product.Price, product.RegularPrice, product.Handle);
}