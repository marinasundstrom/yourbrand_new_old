using Carts.API.Domain.Entities;
using Carts.API.Persistence;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Carts.API.Features.CartsManagement.Requests;

public static class Errors
{
    public readonly static Error CartNotFound = new("cart-not-found", "Cart not found", "");

    public readonly static Error CartItemNotFound = new("cart-not-found", "Cart not found", "");
}

public sealed record GetCarts(int Page = 1, int PageSize = 2) : IRequest<Result<PagedResult<Cart>>>
{
    public sealed class Handler(CartsContext cartsContext = default!) : IRequestHandler<GetCarts, Result<PagedResult<Cart>>>
    {
        public async Task<Result<PagedResult<Cart>>> Handle(GetCarts request, CancellationToken cancellationToken)
        {
            var query = cartsContext.Carts
                .Include(cart => cart.Items.OrderBy(cartItem => cartItem.Created))
                .AsQueryable();

            var total = await query.CountAsync(cancellationToken);

            var carts = await query.OrderBy(x => x.Id)
                .Skip(request.PageSize * (request.Page - 1))
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var pagedResult = new PagedResult<Cart>(carts, total);

            return Result.Success(pagedResult);
        }
    }
}

public sealed record GetCartById(string Id) : IRequest<Result<Cart>>
{
    public sealed class Handler(CartsContext cartsContext) : IRequestHandler<GetCartById, Result<Cart>>
    {
        public async Task<Result<Cart>> Handle(GetCartById request, CancellationToken cancellationToken)
        {
            var cart = await cartsContext.Carts
                .Include(cart => cart.Items.OrderBy(cartItem => cartItem.Created))
                .FirstOrDefaultAsync(cart => cart.Id == request.Id, cancellationToken);

            return cart is not null ? Result.Success(cart) : Result.Failure<Cart>(Errors.CartNotFound);
        }
    }
}

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

public sealed record CreateCart(string Tag) : IRequest<Result<Cart>>
{
    public sealed class Handler(CartsContext cartsContext) : IRequestHandler<CreateCart, Result<Cart>>
    {
        public async Task<Result<Cart>> Handle(CreateCart request, CancellationToken cancellationToken)
        {
            var cart = new Cart(request.Tag);
            cartsContext.Carts.Add(cart);
            await cartsContext.SaveChangesAsync(cancellationToken);

            return Result.Success(cart);
        }
    }
}


public sealed record AddCartItem(string CartId, string Name, string? Image, long? ProductId, string? ProductHandle, string Description, decimal Price, decimal? RegularPrice, int Quantity, string? Data) : IRequest<Result<CartItem>>
{
    public sealed class Handler(CartsContext cartsContext) : IRequestHandler<AddCartItem, Result<CartItem>>
    {
        public async Task<Result<CartItem>> Handle(AddCartItem request, CancellationToken cancellationToken)
        {
            var cart = await cartsContext.Carts
                .Include(cart => cart.Items)
                .FirstOrDefaultAsync(cart => cart.Id == request.CartId, cancellationToken);

            if (cart is null)
            {
                return Result.Failure<CartItem>(Errors.CartNotFound);
            }

            var cartItem = cart.AddItem(request.Name, request.Image, request.ProductId, request.ProductHandle, request.Description, request.Price, request.RegularPrice, request.Quantity, request.Data);

            await cartsContext.SaveChangesAsync(cancellationToken);

            return Result.Success(cartItem);
        }
    }
}

public sealed record UpdateCartItemQuantity(string CartId, string CartItemId, int Quantity) : IRequest<Result<CartItem>>
{
    public sealed class Handler(CartsContext cartsContext) : IRequestHandler<UpdateCartItemQuantity, Result<CartItem>>
    {
        public async Task<Result<CartItem>> Handle(UpdateCartItemQuantity request, CancellationToken cancellationToken)
        {
            var cart = await cartsContext.Carts
                .Include(cart => cart.Items)
                .FirstOrDefaultAsync(cart => cart.Id == request.CartId, cancellationToken);

            if (cart is null)
            {
                return Result.Failure<CartItem>(Errors.CartNotFound);
            }

            var cartItem = cart.Items.FirstOrDefault(x => x.Id == request.CartItemId!);

            if (cartItem is null)
            {
                return Result.Failure<CartItem>(Errors.CartItemNotFound);
            }

            cart.UpdateCartItemQuantity(request.CartItemId, request.Quantity);

            await cartsContext.SaveChangesAsync(cancellationToken);

            return Result.Success(cartItem);
        }
    }
}

public sealed record RemoveCartItem(string CartId, string CartItemId) : IRequest<Result>
{
    public sealed class Handler(CartsContext cartsContext) : IRequestHandler<RemoveCartItem, Result>
    {
        public async Task<Result> Handle(RemoveCartItem request, CancellationToken cancellationToken)
        {
            var cart = await cartsContext.Carts
                .Include(cart => cart.Items)
                .FirstOrDefaultAsync(cart => cart.Id == request.CartId, cancellationToken);

            if (cart is null)
            {
                return Result.Failure<CartItem>(Errors.CartNotFound);
            }

            cart.RemoveItem(request.CartItemId!);

            await cartsContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}

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

public sealed record UpdateCartItemData(string CartId, string CartItemId, string? Data) : IRequest<Result<CartItem>>
{
    public sealed class Validator : AbstractValidator<RemoveCartItem>
    {
        public Validator()
        {
            RuleFor(x => x.CartId).NotEmpty();

            RuleFor(x => x.CartItemId).NotEmpty();
        }
    }

    public sealed class Handler(CartsContext cartsContext) : IRequestHandler<UpdateCartItemData, Result<CartItem>>
    {
        public async Task<Result<CartItem>> Handle(UpdateCartItemData request, CancellationToken cancellationToken)
        {
            var cart = await cartsContext.Carts
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == request.CartId, cancellationToken);

            if (cart is null)
            {
                return Result.Failure<CartItem>(Errors.CartNotFound);
            }

            var cartItem = cart.Items.FirstOrDefault(x => x.Id == request.CartItemId);

            if (cartItem is null)
            {
                throw new System.Exception();
            }

            cartItem.UpdateData(request.Data);

            await cartsContext.SaveChangesAsync(cancellationToken);

            return Result.Success(cartItem);
        }
    }
}
