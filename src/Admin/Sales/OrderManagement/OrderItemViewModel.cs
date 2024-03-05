using System.ComponentModel.DataAnnotations;

using Core;

namespace YourBrand.Admin.Sales.OrderManagement;

public class OrderItemViewModel
{
    public string? Id { get; set; }

    [Required]
    public string Description { get; set; } = null!;

    public string? ItemId { get; set; }

    [Required]
    public decimal Price { get; set; }

    public decimal? RegularPrice { get; set; }

    public decimal? Discount => (RegularPrice is null ? null : RegularPrice.GetValueOrDefault() - Price);

    [Required]
    [Range(0.0001, double.MaxValue)]
    public double Quantity { get; set; } = 1;

    [Required]
    public string Unit { get; set; } = string.Empty;

    public double? VatRate { get; set; } = 0.25;

    public decimal SubTotal => Total - Vat;

    public decimal Vat => Total.GetVatFromTotal(VatRate.GetValueOrDefault());

    public decimal Total => Price * (decimal)Quantity;

    public string? Notes { get; set; }
}
