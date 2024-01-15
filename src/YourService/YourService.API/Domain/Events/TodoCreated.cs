using YourBrand.Domain;

namespace YourBrand.YourService.API.Domain.Events;

public sealed record TodoCreated(string TodoId) : DomainEvent;