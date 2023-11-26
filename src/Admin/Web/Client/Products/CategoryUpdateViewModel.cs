using System.ComponentModel.DataAnnotations;

using AdminAPI;

using MudBlazor;

namespace YourBrand.Client.Products;

public class CategoryUpdateViewModel(IProductsClient productsClient, IProductCategoriesClient productCategoriesClient, ISnackbar snackbar)
{
    public static CategoryUpdateViewModel Create(Product product, IProductsClient productsClient, IProductCategoriesClient productCategoriesClient, ISnackbar snackbar)
    {
        return new(productsClient, productCategoriesClient, snackbar)
        {
            ProductId = product.Id,
            Category = new ParentProductCategory
            {
                Id = product.Category.Id,
                Name = product.Category.Name
            }
        };
    }

    [Required]
    public ParentProductCategory Category { get; set; }

    public long ProductId { get; private set; }

    public async Task<IEnumerable<ParentProductCategory>> Search(string value)
    {
        var result = await productCategoriesClient.GetProductsCategoriesAsync(1, 20, value);
        return result.Items
            .Where(x => x.CanAddProducts)
            .Select(x => new ParentProductCategory
            {
                Id = x.Id,
                Name = x.Name
            });
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
}