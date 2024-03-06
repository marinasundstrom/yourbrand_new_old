using YourBrand.Admin.Sales.OrderManagement;

namespace YourBrand.Admin.Sales;

public class OrderTest
{
    [Fact]
    public void SumOfOrderLines()
    {
        OrderViewModel orderViewModel = new()
        {

        };

        OrderItemViewModel orderItem = new()
        {
            Description = "Item 1",
            Quantity = 3,
            Price = 100m,
            VatRate = 0.25
        };

        orderViewModel.AddItem(orderItem);

        OrderItemViewModel orderItem2 = new()
        {
            Description = "Item 2",
            Quantity = 1,
            Price = 100m,
            VatRate = 0.14
        };

        orderViewModel.AddItem(orderItem2);

        var sumOfOrderItemVat = orderViewModel.Items.Sum(x => x.Vat);

        Assert.Equal(sumOfOrderItemVat, orderViewModel.Vat);

        var sumOfOrderItemTotals = orderViewModel.Items.Sum(x => x.Total);

        Assert.Equal(sumOfOrderItemTotals, orderViewModel.Total);
    }
}