using YourBrand.Admin.Sales.OrderManagement;

namespace YourBrand.Admin.Sales;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        OrderItemViewModel orderItem = new()
        {
            Description = "Test",
            Quantity = 1,
            UnitPrice = 100m,
            VatRate = 0.25
        };

        Assert.Equal(20m, orderItem.Vat);
        Assert.Equal(100m, orderItem.LineTotal);
    }

    [Fact]
    public void Test2()
    {
        OrderItemViewModel orderItem = new()
        {
            Description = "Test",
            Quantity = 2,
            UnitPrice = 100m,
            VatRate = 0.25
        };

        Assert.Equal(40m, orderItem.Vat);
        Assert.Equal(200m, orderItem.LineTotal);
    }

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
            UnitPrice = 100m,
            VatRate = 0.25
        };

        orderViewModel.AddItem(orderItem);

        OrderItemViewModel orderItem2 = new()
        {
            Description = "Item 2",
            Quantity = 1,
            UnitPrice = 100m,
            VatRate = 0.14
        };

        orderViewModel.AddItem(orderItem2);

        var sumOfOrderItemVat = orderViewModel.Items.Sum(x => x.Vat);

        Assert.Equal(sumOfOrderItemVat, orderViewModel.Vat);

        var sumOfOrderItemTotals = orderViewModel.Items.Sum(x => x.LineTotal);

        Assert.Equal(sumOfOrderItemTotals, orderViewModel.Total);
    }
}