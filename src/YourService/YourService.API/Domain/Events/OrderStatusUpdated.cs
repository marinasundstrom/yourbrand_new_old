using YourBrand.YourService.API.Domain.Entities;

namespace YourBrand.YourService.API.Domain.Events;
public sealed record OrderStatusUpdated(string OrderId, int NewStatus, int OldStatus) : DomainEvent;