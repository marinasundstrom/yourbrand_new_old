using System.ComponentModel.DataAnnotations;

using MudBlazor;

using YourBrand.Catalog;

namespace YourBrand.Admin.Sales.Catalog.Products;

public class VatRateUpdateViewModel(IProductsClient productsClient, ISnackbar snackbar)
{
    public static VatRateUpdateViewModel Create(Product product, IProductsClient productsClient, ISnackbar snackbar)
    {
        return new(productsClient, snackbar)
        {
            ProductId = product.Id,
            VatRate = product.VatRate
        };
    }
    public double? VatRate { get; set; }

    public long ProductId { get; private set; }

    public async Task UpdateVatRate()
    {
        try
        {
            await productsClient.UpdateProductVatRateAsync(ProductId.ToString(), new UpdateProductVatRateRequest()
            {
                VatRate = VatRate
            });

            snackbar.Add("Vat Rate was updated", Severity.Info);
        }
        catch
        {
            snackbar.Add("Failed to update product Vat Rate", Severity.Error);
        }
    }
}