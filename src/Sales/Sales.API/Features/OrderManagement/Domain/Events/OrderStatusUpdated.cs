using Sales.API.Features.OrderManagement.Domain.Entities;

namespace Sales.API.Features.OrderManagement.Domain.Events;
public sealed record OrderStatusUpdated(string OrderId, int NewStatus, int OldStatus) : DomainEvent;