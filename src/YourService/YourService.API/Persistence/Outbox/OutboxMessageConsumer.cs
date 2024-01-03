namespace YourBrand.YourService.API.Persistence.Outbox;

public sealed class OutboxMessageConsumer
{
    public required Guid Id { get; set; }

    public required string Consumer { get; set; }
}