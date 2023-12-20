using Sales.API.Features.OrderManagement.Orders.Dtos;

namespace Sales.API.Features.OrderManagement.Orders;

public sealed record CreateOrderRequest(string? CustomerId, BillingDetailsDto BillingDetails, ShippingDetailsDto? ShippingDetails, IEnumerable<CreateOrderItemDto> Items);

public sealed record CreateDraftOrderRequest();