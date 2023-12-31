using MediatR;

namespace YourBrand.Catalog.API.Domain
{
    public abstract record DomainEvent : INotification
    {
        public Guid Id { get; } = Guid.NewGuid();

        public DateTime Timestamp { get; } = DateTime.UtcNow;
    }
}