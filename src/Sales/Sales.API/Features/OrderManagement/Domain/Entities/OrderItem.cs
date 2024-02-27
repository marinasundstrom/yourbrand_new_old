namespace YourBrand.Sales.API.Features.OrderManagement.Domain.Entities;

public class OrderItem : Entity<string>, IAuditable
{
    internal OrderItem(string? itemId,
                       string description,
                       string? unit,
                       decimal price,
                       double? vatRate,
                       decimal? vat,
                       decimal? regularPrice,
                       double? discountRate,
                       decimal? discount,
                       double quantity,
                       decimal total,
                       string? notes)
        : base(Guid.NewGuid().ToString())
    {
        ItemId = itemId;
        Description = description;
        Unit = unit;
        Price = price;
        VatRate = vatRate;
        Vat = vat;
        RegularPrice = regularPrice;
        DiscountRate = discountRate;
        Discount = discount;
        Quantity = quantity;
        Total = total;
        Notes = notes;
    }

    public Order? Order { get; private set; }

    public string? ItemId { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? Unit { get; set; }

    public decimal Price { get; set; }

    public double? VatRate { get; set; }

    public decimal? Vat { get; set; }

    public decimal? RegularPrice { get; set; }

    public double? DiscountRate { get; set; }

    public decimal? Discount { get; set; }

    public double Quantity { get; set; }

    public decimal Total { get; set; }

    public string? Notes { get; set; }

    public User? CreatedBy { get; set; }

    public string? CreatedById { get; set; }

    public DateTimeOffset Created { get; set; }

    public User? LastModifiedBy { get; set; }

    public string? LastModifiedById { get; set; }

    public DateTimeOffset? LastModified { get; set; }
}