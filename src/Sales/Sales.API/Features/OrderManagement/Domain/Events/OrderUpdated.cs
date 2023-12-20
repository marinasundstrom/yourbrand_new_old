namespace Sales.API.Features.OrderManagement.Domain.Events;

public sealed record OrderUpdated(string OrderId) : DomainEvent;