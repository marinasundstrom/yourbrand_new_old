﻿namespace YourBrand.YourService.API.Features.OrderManagement.Orders.Dtos;

public class ShippingDetailsDto
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? CareOf { get; set; }

    public AddressDto Address { get; set; } = null!;
}