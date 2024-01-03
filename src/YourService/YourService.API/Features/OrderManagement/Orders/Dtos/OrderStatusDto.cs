using System.ComponentModel.DataAnnotations;

using YourBrand.Orders.Application;

namespace YourBrand.YourService.API.Features.OrderManagement.Orders.Dtos;

public record OrderStatusDto
(
    int Id,
    string Name,
    string Handle,
    string? Description
);