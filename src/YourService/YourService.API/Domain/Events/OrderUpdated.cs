namespace YourBrand.YourService.API.Domain.Events;

public sealed record OrderUpdated(string OrderId) : DomainEvent;