﻿namespace YourBrand.Domain.Outbox;

public sealed class OutboxMessageConsumer
{
    public required Guid Id { get; set; }

    public required string Consumer { get; set; }
}
