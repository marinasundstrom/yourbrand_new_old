using System.ComponentModel.DataAnnotations;

using CatalogAPI;

using MudBlazor;

namespace YourBrand.Client.Products;

public class CategoryUpdateViewModel(IProductsClient productsClient, IProductCategoriesClient productCategoriesClient, ISnackbar snackbar)
{
    public static CategoryUpdateViewModel Create(Product product, IProductsClient productsClient, IProductCategoriesClient productCategoriesClient, ISnackbar snackbar)
    {
        return new(productsClient, productCategoriesClient, snackbar)
        {
            ProductId = product.Id,
            Category = new ProductCategory(product.Category.Id!, product.Category.Name, true)
        };
    }

    [Required]
    public ProductCategory Category { get; set; }

    public long ProductId { get; private set; }

    public async Task<IEnumerable<ProductCategory>> Search(string value)
    {
        var result = await productCategoriesClient.GetProductCategoriesAsync(1, 20, value);
        return result.Items
            .Where(x => x.CanAddProducts)
            .Select(x => new ProductCategory(x.Id!, x.Name, x.CanAddProducts));
    }

    public async Task UpdateCategory()
    {
        try
        {
            await productsClient.UpdateProductCategoryAsync(ProductId.ToString(), new UpdateProductCategoryRequest()
            {
                ProductCategoryId = Category.Id
            });

            snackbar.Add("Category was updated", Severity.Info);
        }
        catch
        {
            snackbar.Add("Category to update product handle", Severity.Error);
        }
    }

    public record ProductCategory(long Id, string Name, bool CanAddProducts);
}