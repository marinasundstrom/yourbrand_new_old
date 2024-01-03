using YourBrand.Catalog.API.Domain;
using YourBrand.Catalog.API.Domain.Entities;
using YourBrand.Catalog.API.Features.ProductManagement.Products.Variants;

using Core;

namespace YourBrand.Catalog.UnitTests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        Product product = new("", "")
        {
            VatRate = 0.25
        };

        product.SetPrice(100);

        var subTotal = product.Price.GetSubTotal(product.VatRate.GetValueOrDefault());
        var vat = product.Price.GetVatFromTotal(product.VatRate.GetValueOrDefault());

        Assert.True(true);
    }

    [Fact]
    public void Test2()
    {
        Product product = new("", "")
        {
            VatRate = 0.12
        };

        product.SetPrice(99);

        var subTotal = product.Price.GetSubTotal(product.VatRate.GetValueOrDefault());
        var vat = product.Price.GetVatFromTotal(product.VatRate.GetValueOrDefault());

        Assert.True(true);
    }
}