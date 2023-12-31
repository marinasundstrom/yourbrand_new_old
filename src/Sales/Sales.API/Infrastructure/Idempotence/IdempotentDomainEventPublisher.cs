using Azure;

using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Sales.API.Persistence;
using YourBrand.Sales.API.Persistence.Outbox;

namespace YourBrand.Sales.API.Infrastructure.Idempotence;

public class IdempotentDomainEventPublisher : INotificationPublisher
{
    private readonly SalesContext dbContext;

    public IdempotentDomainEventPublisher(
        SalesContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task Publish(IEnumerable<NotificationHandlerExecutor> handlerExecutors, INotification n, CancellationToken cancellationToken)
    {
        if (n is DomainEvent notification)
        {
            foreach (var handler in handlerExecutors)
            {
                string consumer = notification.GetType().Name;

                if (await dbContext.Set<OutboxMessageConsumer>()
                    .AnyAsync(
                        outboxMessageConsumer =>
                            outboxMessageConsumer.Id == notification.Id &&
                            outboxMessageConsumer.Consumer == consumer,
                        cancellationToken))
                {
                    return;
                }

                await handler.HandlerCallback(notification, cancellationToken).ConfigureAwait(false);

                dbContext.Set<OutboxMessageConsumer>()
                    .Add(new OutboxMessageConsumer
                    {
                        Id = notification.Id,
                        Consumer = consumer
                    });

                await dbContext.SaveChangesAsync(cancellationToken);

            }
        }
    }
}