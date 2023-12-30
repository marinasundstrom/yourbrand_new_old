using Carts.API.Domain.Entities;
using Carts.API.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Carts.API.Features.CartsManagement.Requests;

public sealed record GetCartItemById(string CartId, string CartItemId) : IRequest<Result<CartItem>>
{
    public sealed class Handler(CartsContext cartsContext) : IRequestHandler<GetCartItemById, Result<CartItem>>
    {
        public async Task<Result<CartItem>> Handle(GetCartItemById request, CancellationToken cancellationToken)
        {
            var cartItem = await cartsContext.CartItems
                .FirstOrDefaultAsync(cartItem => cartItem.Id == request.CartItemId, cancellationToken);

            if (cartItem is null)
            {
                return Result.Failure<CartItem>(Errors.CartItemNotFound);
            }

            return Result.Success(cartItem);
        }
    }
}
