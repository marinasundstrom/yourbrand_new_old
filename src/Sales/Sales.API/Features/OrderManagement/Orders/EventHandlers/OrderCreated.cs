using System;

using MediatR;

using Sales.API.Features.OrderManagement.Domain.Events;
using Sales.API.Features.OrderManagement.Repositories;

using YourBrand.Orders.Application.Common;
using YourBrand.Orders.Application.Services;

namespace Sales.API.Features.OrderManagement.Orders.EventHandlers;

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