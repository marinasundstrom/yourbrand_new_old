using System.ComponentModel.DataAnnotations;

using CatalogAPI;

using Microsoft.AspNetCore.Components.Forms;

using MudBlazor;

namespace YourBrand.Client.Products;

public class ImageUpdateViewModel(IProductsClient productsClient, ISnackbar snackbar)
{
    public static ImageUpdateViewModel Create(Product product, IProductsClient productsClient, ISnackbar snackbar)
    {
        return new(productsClient, snackbar)
        {
            ProductId = product.Id,
            Image = product.Image
        };
    }

    public string Image { get; set; }

    public long ProductId { get; private set; }

    public async Task UploadProductImage(IBrowserFile file)
    {
        try
        {
            Image = await productsClient.UploadProductImageAsync(ProductId.ToString(), new FileParameter(file.OpenReadStream(3 * 1000000), file.Name));

            snackbar.Add("Image was updated", Severity.Info);
        }
        catch
        {
            snackbar.Add("Failed to update product image", Severity.Error);
        }
    }

}
