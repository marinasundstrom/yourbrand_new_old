namespace YourBrand.Catalog.API.Domain.Entities;

public interface IHasDomainEvents
{
    IReadOnlyCollection<DomainEvent> DomainEvents { get; }

    void AddDomainEvent(DomainEvent domainEvent);

    void RemoveDomainEvent(DomainEvent domainEvent);

    void ClearDomainEvents();
}