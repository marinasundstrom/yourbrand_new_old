﻿namespace YourBrand.YourService.API.Features.OrderManagement.Orders.Dtos;

public sealed class CreateOrderItemDto
{
    public string Description { get; set; } = null!;

    public string? ItemId { get; set; }

    public string? Unit { get; set; }

    public decimal UnitPrice { get; set; }

    public double VatRate { get; set; }

    public double Quantity { get; set; }

    public string? Notes { get; set; }
}