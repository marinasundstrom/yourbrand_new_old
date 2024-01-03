﻿using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.YourService.API.Features.OrderManagement.Orders.Dtos;

namespace YourBrand.YourService.API.Features.OrderManagement.Orders.Statuses.Commands;

public record CreateOrderStatusCommand(string Name, string Handle, string? Description) : IRequest<OrderStatusDto>
{
    public class CreateOrderStatusCommandHandler : IRequestHandler<CreateOrderStatusCommand, OrderStatusDto>
    {
        private readonly IAppDbContext context;

        public CreateOrderStatusCommandHandler(IAppDbContext context)
        {
            this.context = context;
        }

        public async Task<OrderStatusDto> Handle(CreateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var orderStatus = await context.OrderStatuses.FirstOrDefaultAsync(i => i.Name == request.Name, cancellationToken);

            if (orderStatus is not null) throw new Exception();

            orderStatus = new Domain.Entities.OrderStatus(request.Name, request.Handle, request.Description);

            context.OrderStatuses.Add(orderStatus);

            await context.SaveChangesAsync(cancellationToken);

            return orderStatus.ToDto();
        }
    }
}