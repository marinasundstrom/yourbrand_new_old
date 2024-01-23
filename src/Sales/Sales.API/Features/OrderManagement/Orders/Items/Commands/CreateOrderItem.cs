using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Domain.Infrastructure;
using YourBrand.Sales.API.Features.OrderManagement.Orders.Dtos;
using YourBrand.Sales.API.Features.OrderManagement.Repositories;

using static YourBrand.Sales.API.Results;
using static YourBrand.Sales.API.Features.OrderManagement.Domain.Errors.Orders;

namespace YourBrand.Sales.API.Features.OrderManagement.Orders.Items.Commands;

public sealed record CreateOrderItem(string OrderId, string Description, string? ItemId, string? Unit, decimal UnitPrice, double VatRate, double Quantity, string? Notes) : IRequest<Result<OrderItemDto>>
{
    public sealed class Validator : AbstractValidator<CreateOrderItem>
    {
        public Validator()
        {
            RuleFor(x => x.OrderId);

            RuleFor(x => x.Description).NotEmpty().MaximumLength(240);
        }
    }

    public sealed class Handler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        : IRequestHandler<CreateOrderItem, Result<OrderItemDto>>
    {
        private readonly IOrderRepository orderRepository = orderRepository;
        private readonly IUnitOfWork unitOfWork = unitOfWork;

        public async Task<Result<OrderItemDto>> Handle(CreateOrderItem request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.FindByIdAsync(request.OrderId, cancellationToken);

            if (order is null)
            {
                return OrderNotFound;
            }

            var orderItem = order.AddOrderItem(request.Description, request.ItemId, request.Unit, request.UnitPrice, request.VatRate, request.Quantity, request.Notes);

            order.Calculate();

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return orderItem!.ToDto();
        }
    }
}
