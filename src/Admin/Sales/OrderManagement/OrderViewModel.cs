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

    public IEnumerable<OrderItemViewModel> Items => _items;

    public void AddItem(OrderItemViewModel item)
    {
        _items.Add(item);

        Update();
    }

    public void RemoveItem(OrderItemViewModel item)
    {
        _items.Remove(item);

        Update();
    }

    public List<OrderVatAmountViewModel> VatAmounts => _vatAmounts;

    public decimal SubTotal => Items.Select(i => !VatIncluded ? i.Total : i.Total.GetSubTotal(i.VatRate.GetValueOrDefault())).Sum();

    public decimal Vat => Items.Select(i => VatIncluded ? i.Total.GetVatFromTotal(i.VatRate.GetValueOrDefault()) : i.Total.AddVat(i.VatRate.GetValueOrDefault())).Sum();

    public decimal Total
    {
        get
        {
            var total = Items.Select(i => VatIncluded ? i.Total : i.Total.AddVat(i.VatRate.GetValueOrDefault())).Sum();
            return total;
        }
    }

    public decimal Discount => Items.Sum(x => x.Discount.GetValueOrDefault());

    public decimal? Paid { get; set; }

    public void Update()
    {
        UpdateVatAmounts();
    }

    private void UpdateVatAmounts()
    {
        VatAmounts.ForEach(x =>
        {
            x.Vat = 0;
            x.SubTotal = 0;
            x.Total = 0;
        });

        foreach (var item in Items)
        {
            var vatAmount = VatAmounts.FirstOrDefault(x => x.VatRate == item.VatRate);
            if (vatAmount is null)
            {
                vatAmount = new OrderVatAmountViewModel()
                {
                    VatRate = item.VatRate.GetValueOrDefault(),
                    Name = $"{item.VatRate * 100}%"
                };

                VatAmounts.Add(vatAmount);
            }

            vatAmount.SubTotal += item.Total - item.Vat;
            if (vatAmount.Vat is null && item.Vat > 0)
            {
                vatAmount.Vat = 0;
            }
            vatAmount.Vat += item.Vat;
            vatAmount.Total += item.Total;
        }

        VatAmounts.Sort((x, y) => x.VatRate.GetValueOrDefault().CompareTo(y.VatRate.GetValueOrDefault()));

        VatAmounts.ToList().ForEach(x =>
        {
            if (x.Vat == 0 && x.VatRate != 0)
            {
                VatAmounts.Remove(x);
            }
        });

        var totalVatAmount = VatAmounts.FirstOrDefault(x => x.VatRate == null);

        if (totalVatAmount is null)
        {
            if (VatAmounts.Count == 1)
            {
                return;
            }

            totalVatAmount = new OrderVatAmountViewModel()
            {
                VatRate = null,
                Name = $"Total"
            };

            _vatAmounts.Add(totalVatAmount);
        }

        if (VatAmounts.Count == 1 && totalVatAmount is not null)
        {
            _vatAmounts.Remove(totalVatAmount);
            return;
        }

        totalVatAmount.SubTotal = Items.Sum(x => x.SubTotal);
        totalVatAmount.Vat = Items.Sum(x => x.Vat);
        totalVatAmount.Total = Items.Sum(x => x.Total);
    }
}
public sealed record OrderVatAmountViewModel
{
    public string Name { get; set; }
    public double? VatRate { get; set; }
    public decimal SubTotal { get; set; }
    public decimal? Vat { get; set; }
    public decimal Total { get; set; }
}

public sealed record OrderDiscountViewModel
{
    public decimal Amount { get; set; }
    public string Description { get; set; }
}