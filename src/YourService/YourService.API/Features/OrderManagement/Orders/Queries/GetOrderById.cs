﻿using FluentValidation;

using MediatR;

using YourBrand.YourService.API.Features.OrderManagement.Orders.Dtos;
using YourBrand.YourService.API.Features.OrderManagement.Repositories;

namespace YourBrand.YourService.API.Features.OrderManagement.Orders.Queries;

public record GetOrderById(string Id) : IRequest<Result<OrderDto>>
{
    public class Validator : AbstractValidator<GetOrderById>
    {
        public Validator()
        {
            RuleFor(x => x.Id);
        }
    }

    public class Handler : IRequestHandler<GetOrderById, Result<OrderDto>>
    {
        private readonly IOrderRepository orderRepository;

        public Handler(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public async Task<Result<OrderDto>> Handle(GetOrderById request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.FindByIdAsync(request.Id, cancellationToken);

            if (order is null)
            {
                return Result.Failure<OrderDto>(Errors.Orders.OrderNotFound);
            }

            return Result.Success(order.ToDto());
        }
    }
}