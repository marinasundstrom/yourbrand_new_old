using System.ComponentModel.DataAnnotations;

using CatalogAPI;

using MudBlazor;

namespace YourBrand.Client.Products;

public class PriceUpdateViewModel(IProductsClient productsClient, ISnackbar snackbar)
{
    public static PriceUpdateViewModel Create(Product product, IProductsClient productsClient, ISnackbar snackbar)
    {
        return new(productsClient, snackbar)
        {
            ProductId = product.Id,
            Price = product.Price
        };
    }

    [Range(0, 100000)]
    public decimal Price { get; set; }

    public long ProductId { get; private set; }

    public async Task UpdatePrice()
    {
        try
        {
            await productsClient.UpdateProductPriceAsync(ProductId.ToString(), new UpdateProductPriceRequest()
            {
                Price = Price
            });

            snackbar.Add("Price was updated", Severity.Info);
        }
        catch
        {
            snackbar.Add("Failed to update product price", Severity.Error);
        }
    }

}
