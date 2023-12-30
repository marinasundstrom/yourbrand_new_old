using Carts.API.Domain.Entities;
using Carts.API.Persistence;

using MediatR;

namespace Carts.API.Features.CartsManagement.Requests;

public sealed record CreateCart(string Tag) : IRequest<Result<Cart>>
{
    public sealed class Handler(CartsContext cartsContext) : IRequestHandler<CreateCart, Result<Cart>>
    {
        public async Task<Result<Cart>> Handle(CreateCart request, CancellationToken cancellationToken)
        {
            var cart = new Cart(request.Tag);
            cartsContext.Carts.Add(cart);
            cart.Created = DateTimeOffset.UtcNow;
            await cartsContext.SaveChangesAsync(cancellationToken);

            return Result.Success(cart);
        }
    }
}
