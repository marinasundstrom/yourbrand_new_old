using Core;

namespace YourBrand.Sales.API.Features.OrderManagement.Domain.Entities;

public class OrderItem : Entity<string>, IAuditable
{
    internal OrderItem(string description,
                       string? itemId,
                       decimal price,
                       decimal? regularPrice,
                       double? discountRate,
                       decimal? discount,
                       double quantity,
                       string? unit,
                       double? vatRate,
                       string? notes)
        : base(Guid.NewGuid().ToString())
    {
        ItemId = itemId;
        Description = description;
        Unit = unit;
        Price = price;
        VatRate = vatRate;
        RegularPrice = regularPrice;
        DiscountRate = discountRate;
        Discount = discount;
        Quantity = quantity;
        Notes = notes;

        Update();
    }

    public Order? Order { get; private set; }

    public string Description { get; set; } = null!;

    public string? ItemId { get; set; } = null!;

    public string? Unit { get; set; }

    public decimal Price { get; set; }

    public decimal? RegularPrice { get; set; }

    public double? DiscountRate { get; set; }

    public decimal? Discount { get; set; }

    public double Quantity { get; set; }

    public decimal SubTotal { get; private set; }

    public double? VatRate { get; set; }

    public decimal? Vat { get; private set; }

    public decimal Total { get; private set; }

    public string? Notes { get; set; }

    public User? CreatedBy { get; set; }

    public string? CreatedById { get; set; }

    public DateTimeOffset Created { get; set; }

    public User? LastModifiedBy { get; set; }

    public string? LastModifiedById { get; set; }

    public DateTimeOffset? LastModified { get; set; }

    public void Update()
    {
        Total = Price * (decimal)Quantity;
        Vat = Math.Round(PriceCalculations.GetVatFromTotal(Total, VatRate.GetValueOrDefault()), 2, MidpointRounding.ToEven);
    }
}