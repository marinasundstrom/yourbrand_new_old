using Catalog.API.Domain;
using Catalog.API.Domain.Entities;
using Catalog.API.Features.ProductManagement.Products.Variants;

using Core;

namespace Catalog.UnitTests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        Product product = new("", "")
        {
            Price = 100,
            VatRate = 0.25
        };

        var subTotal = product.Price.GetSubTotal(product.VatRate.GetValueOrDefault());
        var vat = product.Price.GetVatFromTotal(product.VatRate.GetValueOrDefault());

        Assert.True(true);
    }

    [Fact]
    public void Test2()
    {
        Product product = new("", "")
        {
            Price = 90,
            VatRate = 0.12
        };

        var subTotal = product.Price.GetSubTotal(product.VatRate.GetValueOrDefault());
        var vat = product.Price.GetVatFromTotal(product.VatRate.GetValueOrDefault());

        Assert.True(true);
    }
}