using System.ComponentModel.DataAnnotations;

using Core;

using YourBrand.Sales;

namespace YourBrand.Admin.Sales.OrderManagement;

public class OrderViewModel
{
    private List<OrderItemViewModel> _items = new List<OrderItemViewModel>();
    private List<OrderVatAmountViewModel> _vatAmounts = new List<OrderVatAmountViewModel>();

    public int Id { get; set; }

    [Required]
    public DateTime? Date { get; set; }

    [Required]
    public TimeSpan? Time { get; set; }

    [Required]
    public OrderStatus Status { get; set; }

    public string? Reference { get; set; }

    public string? Notes { get; set; }

    public DateTime? DueDate { get; set; }

    public bool VatIncluded { get; set; } = true;

    public IEnumerable<OrderItemViewModel> Items => _items ??= new List<OrderItemViewModel>();

    public void AddItem(OrderItemViewModel item)
    {
        _items.Add(item);

        Calculate();
    }

    public void RemoveItem(OrderItemViewModel item)
    {
        _items.Remove(item);

        Calculate();
    }

    public List<OrderVatAmountViewModel> VatAmounts => _vatAmounts;

    public decimal SubTotal => Items.Select(i => !VatIncluded ? i.LineTotal : i.LineTotal.GetSubTotal(i.VatRate.GetValueOrDefault())).Sum();

    public decimal Vat => Items.Select(i => VatIncluded ? i.LineTotal.GetVatFromTotal(i.VatRate.GetValueOrDefault()) : i.LineTotal.AddVat(i.VatRate.GetValueOrDefault())).Sum();

    public decimal Total
    {
        get
        {
            var total = Items.Select(i => VatIncluded ? i.LineTotal : i.LineTotal.AddVat(i.VatRate.GetValueOrDefault())).Sum();
            return total;
        }
    }

    public decimal? Paid { get; set; }

    public void Calculate()
    {
        VatAmounts.ForEach(x => x.Amount = 0);

        foreach (var item in Items)
        {
            //item.Total = item.Price * (decimal)item.Quantity;
            //item.Vat = Math.Round(PriceCalculations.GetVatFromTotal(item.Total, item.VatRate.GetValueOrDefault()), 2, MidpointRounding.ToEven);

            if (item.VatRate is null)
            {
                continue;
            }

            var vatAmount = VatAmounts.FirstOrDefault(x => x.VatRate == item.VatRate);
            if (vatAmount is null)
            {
                vatAmount = new OrderVatAmountViewModel()
                {
                    VatRate = item.VatRate.GetValueOrDefault(),
                    Name = $"{(item.VatRate * 100)}%"
                };

                VatAmounts.Add(vatAmount);
            }

            vatAmount.Amount += item.Vat;
        }

        VatAmounts.ToList().ForEach(x =>
        {
            if (x.Amount == 0)
            {
                VatAmounts.Remove(x);
            }
        });

        var totalVatAmount = VatAmounts.FirstOrDefault(x => x.VatRate == 0);

        if (totalVatAmount is null)
        {
            if (VatAmounts.Count == 1)
            {
                return;
            }

            totalVatAmount = new OrderVatAmountViewModel()
            {
                VatRate = 0,
                Name = $"Total"
            };

            _vatAmounts.Insert(0, totalVatAmount);
        }

        if (VatAmounts.Count == 1 && totalVatAmount is not null)
        {
            _vatAmounts.Remove(totalVatAmount);
            return;
        }

        totalVatAmount.Amount = VatAmounts.Sum(x => x.Amount);
    }
}
public sealed record OrderVatAmountViewModel
{
    public string Name { get; set; }
    public double VatRate { get; set; }
    public decimal Amount { get; set; }
}

public sealed record OrderDiscountViewModel
{
    public decimal Amount { get; set; }
    public string Description { get; set; }
}