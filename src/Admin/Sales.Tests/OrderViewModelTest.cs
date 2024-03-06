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
            Price = 100m,
            VatRate = 0.25,
            Quantity = 3
        };

        orderViewModel.AddItem(orderItem);

        OrderItemViewModel orderItem2 = new()
        {
            Description = "Item 2",
            Price = 100m,
            VatRate = 0.14,
            Quantity = 1
        };

        orderViewModel.AddItem(orderItem2);

        var sumOfOrderItemVat = orderViewModel.Items.Sum(x => x.Vat);

        Assert.Equal(sumOfOrderItemVat, orderViewModel.Vat);

        var sumOfOrderItemTotals = orderViewModel.Items.Sum(x => x.Total);

        Assert.Equal(sumOfOrderItemTotals, orderViewModel.Total);
    }

    [Fact]
    public void VatTotals()
    {
        OrderViewModel orderViewModel = new()
        {

        };

        OrderItemViewModel orderItem = new()
        {
            Description = "Item 1",
            Price = 100m,
            VatRate = 0.25,
            Quantity = 3
        };

        orderViewModel.AddItem(orderItem);

        Assert.Single(orderViewModel.VatAmounts);

        Assert.Contains(orderViewModel.VatAmounts, x => x.VatRate == 0.25);
    }

    [Fact]
    public void VatTotals2()
    {
        OrderViewModel orderViewModel = new()
        {

        };

        OrderItemViewModel orderItem = new()
        {
            Description = "Item 1",
            Price = 100m,
            VatRate = 0.25,
            Quantity = 3
        };

        orderViewModel.AddItem(orderItem);

        OrderItemViewModel orderItem2 = new()
        {
            Description = "Item 2",
            Price = 100m,
            VatRate = 0.14,
            Quantity = 1
        };

        orderViewModel.AddItem(orderItem2);

        Assert.Equal(3, orderViewModel.VatAmounts.Count);

        Assert.Contains(orderViewModel.VatAmounts, x => x.VatRate == 0.25);

        Assert.Contains(orderViewModel.VatAmounts, x => x.VatRate == 0.14);

        Assert.Contains(orderViewModel.VatAmounts, x => x.VatRate == 0);
    }
}