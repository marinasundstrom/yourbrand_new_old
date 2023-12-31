using YourBrand.Carts.API.Domain.Entities;
using YourBrand.Carts.API.Persistence;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Carts.API.Features.CartsManagement.Requests;

public record GetCartByTag(string Tag) : IRequest<Result<Cart>>
{
    public class Validator : AbstractValidator<GetCartById>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public class Handler(CartsContext cartsContext) : IRequestHandler<GetCartByTag, Result<Cart>>
    {
        public async Task<Result<Cart>> Handle(GetCartByTag request, CancellationToken cancellationToken)
        {
            var cart = await cartsContext.Carts
                .Include(cart => cart.Items.OrderBy(cartItem => cartItem.Created))
                .FirstOrDefaultAsync(cart => cart.Tag == request.Tag, cancellationToken);

            if (cart is null)
            {
                return Result.Failure<Cart>(Errors.CartNotFound);
            }

            return Result.Success(cart);
        }
    }
}
