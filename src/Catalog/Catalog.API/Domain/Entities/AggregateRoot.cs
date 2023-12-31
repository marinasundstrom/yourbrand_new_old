namespace YourBrand.Catalog.API.Domain.Entities;

public abstract class AggregateRoot<TId> : Entity<TId>
    where TId : notnull
{
    protected AggregateRoot() : base()
    {
    }

    protected AggregateRoot(TId id) : base(id)
    {
    }
}