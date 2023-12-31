using Microsoft.EntityFrameworkCore;

using YourBrand.Sales.API.Persistence;
using YourBrand.Sales.API.Persistence.Outbox;

using YourBrand.Orders.Application.Common;

namespace YourBrand.Sales.API.Infrastructure.Idempotence;

public sealed class IdempotentDomainEventHandler<TDomainEvent> : IDomainEventHandler<TDomainEvent>
    where TDomainEvent : DomainEvent
{
    private readonly IDomainEventHandler<TDomainEvent> decorated;
    private readonly SalesContext dbContext;

    public IdempotentDomainEventHandler(
        IDomainEventHandler<TDomainEvent> decorated,
        SalesContext dbContext)
    {
        this.decorated = decorated;
        this.dbContext = dbContext;
    }

    public async Task Handle(TDomainEvent notification, CancellationToken cancellationToken)
    {
        string consumer = decorated.GetType().Name;

        Console.WriteLine("Consumer: " + consumer);

        if (await dbContext.Set<OutboxMessageConsumer>()
            .AnyAsync(
                outboxMessageConsumer =>
                    outboxMessageConsumer.Id == notification.Id &&
                    outboxMessageConsumer.Consumer == consumer,
                cancellationToken))
        {
            return;
        }

        await decorated.Handle(notification, cancellationToken);

        dbContext.Set<OutboxMessageConsumer>()
            .Add(new OutboxMessageConsumer
            {
                Id = notification.Id,
                Consumer = consumer
            });

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

