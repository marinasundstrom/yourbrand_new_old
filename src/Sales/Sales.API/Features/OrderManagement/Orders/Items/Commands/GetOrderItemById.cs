using MediatR;

using Sales.API.Features.OrderManagement.Orders.Dtos;
using Sales.API.Features.OrderManagement.Repositories;

namespace Sales.API.Features.OrderManagement.Orders.Items.Commands;

public sealed record GetOrderItemById(string OrderId, string OrderItemId) : IRequest<Result<OrderItemDto>>
{
    public sealed class Handler(IOrderRepository orderRepository, IUnitOfWork unitOfWork) : IRequestHandler<GetOrderItemById, Result<OrderItemDto>>
    {
        public async Task<Result<OrderItemDto>> Handle(GetOrderItemById request, CancellationToken cancellationToken)
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

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(orderItem.ToDto());
        }
    }
}