using System.Collections.Generic;

using YourBrand.Sales.API;
using YourBrand.Sales.API.Features.OrderManagement.Domain.Events;
using YourBrand.Sales.API.Features.OrderManagement.Domain.ValueObjects;

using Core;

namespace YourBrand.Sales.API.Features.OrderManagement.Domain.Entities;

public class Order : AggregateRoot<string>, IAuditable
{
    readonly HashSet<OrderItem> _items = new HashSet<OrderItem>();

    public Order() : base(Guid.NewGuid().ToString())
    {
        StatusId = 1;
    }

    public int OrderNo { get; set; }

    public string CompanyId { get; private set; } = "ACME";

    public DateTime Date { get; private set; } = DateTime.Now;

    public OrderStatus Status { get; private set; } = null!;

    public int StatusId { get; set; } = 1;

    public bool UpdateStatus(int status)
    {
        var oldStatus = StatusId;
        if (status != oldStatus)
        {
            StatusId = status;

            AddDomainEvent(new OrderUpdated(Id));
            AddDomainEvent(new OrderStatusUpdated(Id, status, oldStatus));

            return true;
        }

        return false;
    }

    public User? Assignee { get; private set; }

    public string? AssigneeId { get; private set; }

    public bool UpdateAssigneeId(string? userId)
    {
        var oldAssigneeId = AssigneeId;
        if (userId != oldAssigneeId)
        {
            AssigneeId = userId;
            //AddDomainEvent(new OrderAssignedUserUpdated(OrderNo, userId, oldAssigneeId));

            return true;
        }

        return false;
    }

    public string? CustomerId { get; set; }

    public bool VatIncluded { get; set; }

    public string Currency { get; set; } = "SEK";

    public decimal SubTotal { get; set; }

    public double VatRate { get; set; }

    public decimal? Vat { get; set; }

    public decimal Discount { get; set; }

    public decimal Total { get; set; }

    public ValueObjects.BillingDetails? BillingDetails { get; set; }

    public ValueObjects.ShippingDetails? ShippingDetails { get; set; }

    public IReadOnlyCollection<OrderItem> Items => _items;

    public OrderItem AddOrderItem(string? itemId,
                       string description,
                       string? unit,
                       decimal price,
                       double? vatRate,
                       decimal? vat,
                       decimal? regularPrice,
                       double? discountRate,
                       decimal? discount,
                       double quantity,
                       string? notes)
    {
        var orderItem = new OrderItem(itemId, description, unit, price, vatRate, vat, regularPrice, discountRate, discount, quantity, price * (decimal)quantity, notes);
        _items.Add(orderItem);
        return orderItem;
    }

    public void RemoveOrderItem(OrderItem orderItem) => _items.Remove(orderItem);

    public void Calculate()
    {
        foreach (var item in Items)
        {
            item.Total = item.Price * (decimal)item.Quantity;
        }

        VatRate = 0.25;
        Vat = VatIncluded ? Items.Select(x => x.Total.GetVatFromTotal(x.VatRate.GetValueOrDefault())).Sum() : Items.Sum(x => (decimal)x.VatRate.GetValueOrDefault() * x.Total);
        Total = Items.Sum(x => x.Total);
        SubTotal = (VatIncluded ? (Total - Vat.GetValueOrDefault()) : Total);
    }

    public User? CreatedBy { get; set; }

    public string? CreatedById { get; set; }

    public DateTimeOffset Created { get; set; }

    public User? LastModifiedBy { get; set; }

    public string? LastModifiedById { get; set; }

    public DateTimeOffset? LastModified { get; set; }
}