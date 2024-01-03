using System;

using MediatR;

using YourBrand.YourService.API.Domain.Events;
using YourBrand.YourService.API.Features.OrderManagement.Repositories;

using YourBrand.YourService.API.Common;
using YourBrand.YourService.API.Services;

namespace YourBrand.YourService.API.Features.OrderManagement.Orders.EventHandlers;

public sealed class OrderCreatedEventHandler : IDomainEventHandler<OrderCreated>
{
    private readonly IOrderRepository orderRepository;
    private readonly IOrderNotificationService orderNotificationService;

    public OrderCreatedEventHandler(IOrderRepository orderRepository, IOrderNotificationService orderNotificationService)
    {
        this.orderRepository = orderRepository;
        this.orderNotificationService = orderNotificationService;
    }

    public async Task Handle(OrderCreated notification, CancellationToken cancellationToken)
    {
        var order = await orderRepository.FindByIdAsync(notification.OrderId, cancellationToken);

        if (order is null)
            return;

        Console.WriteLine("CREATED C");

        //await orderNotificationService.Created(order.OrderNo);
    }
}