using System.ComponentModel.DataAnnotations;

using AdminAPI;

using MudBlazor;

namespace YourBrand.Client.Products;

public class VisibilityUpdateViewModel(IProductsClient productsClient, ISnackbar snackbar)
{
    public static VisibilityUpdateViewModel Create(Product product, IProductsClient productsClient, ISnackbar snackbar)
    {
        return new(productsClient, snackbar)
        {
            ProductId = product.Id,
            Visibility = product.Visibility
        };
    }

    [Required]
    public ProductVisibility Visibility { get; set; }

    public long ProductId { get; private set; }

    public async Task UpdateVisibility()
    {
        try
        {
            await productsClient.UpdateProductVisibilityAsync(ProductId.ToString(), new UpdateProductVisibilityRequest()
            {
                Visibility = Visibility
            });

            snackbar.Add("Visibility was updated", Severity.Info);
        }
        catch
        {
            snackbar.Add("Failed to update product visibility", Severity.Error);
        }
    }
}
