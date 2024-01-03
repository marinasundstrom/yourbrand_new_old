using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.YourService.API.Features.OrderManagement.Orders.Dtos;
using YourBrand.YourService.API.Features.OrderManagement.Repositories;
using YourBrand.YourService.API.Persistence;

namespace YourBrand.YourService.API.Features.OrderManagement.Orders.Items.Commands;

public sealed record UpdateOrderItem(string OrderId, string OrderItemId, string Description, string? ItemId, string? Unit, decimal UnitPrice, double VatRate, double Quantity, string? Notes) : IRequest<Result<OrderItemDto>>
{
    public sealed class Validator : AbstractValidator<UpdateOrderItem>
    {
        public Validator()
        {
            RuleFor(x => x.OrderId);

            RuleFor(x => x.Description).NotEmpty().MaximumLength(240);
        }
    }

    public sealed class Handler : IRequestHandler<UpdateOrderItem, Result<OrderItemDto>>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IDomainEventDispatcher domainEventDispatcher;

        public Handler(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IDomainEventDispatcher domainEventDispatcher)
        {
            this.orderRepository = orderRepository;
            this.unitOfWork = unitOfWork;
            this.domainEventDispatcher = domainEventDispatcher;
        }

        public async Task<Result<OrderItemDto>> Handle(UpdateOrderItem request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.FindByIdAsync(request.OrderId, cancellationToken);

            if (order is null)
            {
                return Result.Failure<OrderItemDto>(Errors.Orders.OrderNotFound);
            }

            var orderItem = order.Items.FirstOrDefault(x => x.Id == request.OrderItemId);

            if (orderItem is null)
            {
                return Result.Failure<OrderItemDto>(Errors.Orders.OrderItemNotFound);
            }

            orderItem.Description = request.Description;
            orderItem.ItemId = request.ItemId;
            orderItem.Unit = request.Unit;
            orderItem.UnitPrice = request.UnitPrice;
            orderItem.VatRate = request.VatRate;
            orderItem.Quantity = request.Quantity;
            orderItem.Notes = request.Notes;

            order.Calculate();

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(orderItem!.ToDto());
        }
    }
}