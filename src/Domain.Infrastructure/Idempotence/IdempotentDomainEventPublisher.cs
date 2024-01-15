using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Domain.Outbox;
using YourBrand.Domain.Persistence;

namespace YourBrand.Domain.Infrastructure.Idempotence;

public class IdempotentDomainEventPublisher : INotificationPublisher
{
    private readonly DomainDbContext dbContext;

    public IdempotentDomainEventPublisher(
        DomainDbContext dbContext)
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