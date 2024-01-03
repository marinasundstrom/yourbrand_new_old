namespace YourBrand.YourService.API.Domain.Events;

public sealed record OrderDeleted(int OrderNo) : DomainEvent;