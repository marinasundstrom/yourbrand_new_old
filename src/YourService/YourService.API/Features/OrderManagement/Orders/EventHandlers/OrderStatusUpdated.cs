using MediatR;

using YourBrand.YourService.API.Domain.Events;
using YourBrand.YourService.API.Features.OrderManagement.Orders.Dtos;
using YourBrand.YourService.API.Features.OrderManagement.Repositories;

using YourBrand.YourService.API.Common;
using YourBrand.YourService.API.Services;

namespace YourBrand.YourService.API.Features.OrderManagement.Orders.EventHandlers;

public sealed class OrderStatusUpdatedEventHandler : IDomainEventHandler<OrderStatusUpdated>
{
    private readonly IOrderRepository orderRepository;
    private readonly ICurrentUserService currentUserService;
    private readonly IEmailService emailService;
    private readonly IOrderNotificationService orderNotificationService;

    public OrderStatusUpdatedEventHandler(IOrderRepository orderRepository, ICurrentUserService currentUserService, IEmailService emailService, IOrderNotificationService orderNotificationService)
    {
        this.orderRepository = orderRepository;
        this.currentUserService = currentUserService;
        this.emailService = emailService;
        this.orderNotificationService = orderNotificationService;
    }

    public async Task Handle(OrderStatusUpdated notification, CancellationToken cancellationToken)
    {
        var order = await orderRepository.FindByIdAsync(notification.OrderId, cancellationToken);

        if (order is null)
            return;

        await orderNotificationService.StatusUpdated(order.OrderNo, order.Status.ToDto());

        if (order.AssigneeId is not null && order.LastModifiedById != order.AssigneeId)
        {/*
            await emailService.SendEmail(order.AssigneeId!.Email,
                $"Status of \"{order.Title}\" [{order.OrderNo}] changed to {notification.NewStatus}.",
                $"{order.LastModifiedBy!.Name} changed status of \"{order.Title}\" [{order.OrderNo}] from {notification.OldStatus} to {notification.NewStatus}."); */
        }
    }
}