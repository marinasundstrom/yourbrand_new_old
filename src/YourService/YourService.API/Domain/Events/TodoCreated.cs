using YourBrand.Domain;
using YourBrand.YourService.API.Domain.ValueObjects;

namespace YourBrand.YourService.API.Domain.Events;

public sealed record TodoCreated(TodoId TodoId) : DomainEvent;