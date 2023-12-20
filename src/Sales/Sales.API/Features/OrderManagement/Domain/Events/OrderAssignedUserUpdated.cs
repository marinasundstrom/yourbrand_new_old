namespace Sales.API.Features.OrderManagement.Domain.Events;

public sealed record OrderAssignedUserUpdated(string OrderId, string? AssignedUserId, string? OldAssignedUserId) : DomainEvent;