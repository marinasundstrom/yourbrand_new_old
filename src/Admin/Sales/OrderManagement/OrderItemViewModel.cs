using System.ComponentModel.DataAnnotations;

using Core;

namespace YourBrand.Admin.Sales.OrderManagement;

public class OrderItemViewModel
{
    public string? Id { get; set; }

    [Required]
    public string Description { get; set; } = null!;

    public string? ItemId { get; set; }

    public string? Notes { get; set; }

    [Required]
    public decimal UnitPrice { get; set; }

    [Required]
    public string Unit { get; set; } = string.Empty;

    [Required]
    [Range(0.0001, double.MaxValue)]
    public double Quantity { get; set; } = 1;

    public double? VatRate { get; set; } = 0.25;

    public decimal? Discount { get; set; }

    public decimal SubTotal => LineTotal - Vat;

    public decimal Vat => LineTotal.GetVatFromTotal(VatRate.GetValueOrDefault());

    public decimal LineTotal => UnitPrice * (decimal)Quantity;
}
