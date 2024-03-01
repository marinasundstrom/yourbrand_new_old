﻿using FluentValidation;

using MediatR;

using YourBrand.Sales.API.Features.OrderManagement.Repositories;

using static YourBrand.Sales.API.Results;
using static YourBrand.Sales.API.Features.OrderManagement.Domain.Errors.Orders;

namespace YourBrand.Sales.API.Features.OrderManagement.Orders.Items.Commands;

public sealed record RemoveOrderItem(string OrderId, string OrderItemId) : IRequest<Result>
{
    public sealed class Validator : AbstractValidator<RemoveOrderItem>
    {
        public Validator()
        {
            RuleFor(x => x.OrderId).NotEmpty();

            RuleFor(x => x.OrderItemId).NotEmpty();
        }
    }

    public sealed class Handler(IOrderRepository orderRepository, IUnitOfWork unitOfWork) : IRequestHandler<RemoveOrderItem, Result>
    {
        private readonly IOrderRepository orderRepository = orderRepository;
        private readonly IUnitOfWork unitOfWork = unitOfWork;

        public async Task<Result> Handle(RemoveOrderItem request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.FindByIdAsync(request.OrderId, cancellationToken);

            if (order is null)
            {
                return OrderNotFound;
            }

            var orderItem = order.Items.FirstOrDefault(x => x.Id == request.OrderItemId);

            if (orderItem is null)
            {
                return OrderItemNotFound;
            }

            order.RemoveOrderItem(orderItem);

            order.Update();

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Success;
        }
    }
}