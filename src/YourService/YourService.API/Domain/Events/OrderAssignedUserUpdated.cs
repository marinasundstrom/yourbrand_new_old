namespace YourBrand.YourService.API.Domain.Events;

public sealed record OrderAssignedUserUpdated(string OrderId, string? AssignedUserId, string? OldAssignedUserId) : DomainEvent;