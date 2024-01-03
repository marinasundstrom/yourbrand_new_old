namespace YourBrand.YourService.API.Domain.Events;

public sealed record ProductPriceChanged(string ProductId, decimal Price, decimal? RegularPrice) : DomainEvent;