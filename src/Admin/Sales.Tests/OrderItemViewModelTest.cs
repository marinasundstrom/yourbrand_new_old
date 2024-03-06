using YourBrand.Admin.Sales.OrderManagement;

namespace YourBrand.Admin.Sales;

public class OrderItemViewModelTest
{
    [Theory]
    [InlineData(100, 0, 1, 0, 100)]
    [InlineData(100, 0.06, 1, 5.66, 100)]
    [InlineData(100, 0.12, 1, 10.71, 100)]
    [InlineData(100, 0.25, 1, 20, 100)]
    [InlineData(100, 0.25, 2, 40, 200)]
    public void TestPriceCalculation(decimal price, double vatRate, double quantity, decimal vat, decimal total)
    {
        OrderItemViewModel orderItem = new()
        {
            Description = "Test",
            Price = price,
            VatRate = vatRate,
            Quantity = quantity
        };

        Assert.Equal(vat, orderItem.Vat);
        Assert.Equal(total, orderItem.Total);
    }
}