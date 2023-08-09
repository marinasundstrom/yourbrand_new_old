using Carts.Contracts;
using MediatR;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Carts.API.Consumers;

public sealed class GetCartsConsumer(MediatR.IMediator mediator) : IConsumer<Carts.Contracts.GetCarts>
{
    public async Task Consume(ConsumeContext<Carts.Contracts.GetCarts> context)
    {
        var page = context.Message.Page;
        var pageSize = context.Message.PageSize;

        var r = await mediator.Send(new Requests.GetCarts(page, pageSize), context.CancellationToken);
        var cartsResult = r.GetValue();

        var result = new PagedCartResult {
            Items = cartsResult.Items.Select(x => x.Map()), 
            Total = cartsResult.Total
        };

        await context.RespondAsync<PagedCartResult>(result);
    }
}

public sealed class GetCartByIdConsumer(MediatR.IMediator mediator) : IConsumer<GetCartById>
{
    public async Task Consume(ConsumeContext<Carts.Contracts.GetCartById> context)
    {
        var id = context.Message.Id;

        var r = await mediator.Send(new Requests.GetCartById(id), context.CancellationToken);
        var cart = r.GetValue();

        await context.RespondAsync<GetCartByIdResponse>(new GetCartByIdResponse { Cart = cart.Map() });
    }
}

public sealed class AddCartItemConsumer(MediatR.IMediator mediator) : IConsumer<Carts.Contracts.AddCartItem>
{
    public async Task Consume(ConsumeContext<Carts.Contracts.AddCartItem> context)
    {
        var request = context.Message;

        var r = await mediator.Send(new Requests.AddCartItem(
            request.CartId,
            request.Name,
            request.Image,
            request.ProductId,
            request.Description,
            request.Price,
            request.RegularPrice,
            request.Quantity
        ), context.CancellationToken);

        var cartItem = r.GetValue();

        await context.RespondAsync<AddCartItemResponse>(new AddCartItemResponse {  CartItem = cartItem.Map() });
    }
}

public sealed class UpdateCartItemQuantityConsumer(MediatR.IMediator mediator) : IConsumer<UpdateCartItemQuantity>
{
    public async Task Consume(ConsumeContext<Carts.Contracts.UpdateCartItemQuantity> context)
    {
        var cartId = context.Message.CartId;
        var cartItemId = context.Message.CartItemId;
        var quantity = context.Message.Quantity;

        var r = await mediator.Send(new Requests.UpdateCartItemQuantity(cartId, cartItemId, quantity), context.CancellationToken);
        var cartItem = r.GetValue();

        await context.RespondAsync<UpdateCartItemQuantityResponse>(new UpdateCartItemQuantityResponse { CartItem = cartItem.Map() });
    }
}

public sealed class RemoveCartItemQuantityConsumer(MediatR.IMediator mediator) : IConsumer<RemoveCartItem>
{
    public async Task Consume(ConsumeContext<Carts.Contracts.RemoveCartItem> context)
    {
        var cartId = context.Message.CartId;
        var cartItemId = context.Message.CartItemId;

        var r = await mediator.Send(new Requests.RemoveCartItem(cartId, cartItemId), context.CancellationToken);

        await context.RespondAsync<RemoveCartItemResponse>(new RemoveCartItemResponse());
    }
}

public static class Mappings 
{
    public static Carts.Contracts.Cart Map(this Carts.API.Model.Cart cart) => new Carts.Contracts.Cart {
        Id = cart.Id, 
        Name = cart.Name, 
        Total = cart.Total, 
        Items = cart.Items.Select(cartItem => cartItem.Map()) 
    };

    public static Carts.Contracts.CartItem Map(this Carts.API.Model.CartItem cartItem) => new Carts.Contracts.CartItem {
        Id = cartItem.Id, 
        Name = cartItem.Name, 
        Image = cartItem.Image, 
        ProductId = cartItem.ProductId, 
        Description = cartItem.Description, 
        Price = cartItem.Price, 
        RegularPrice = cartItem.RegularPrice, 
        Quantity = cartItem.Quantity, 
        Total = cartItem.Total, 
        Created = cartItem.Created
    };
}

public sealed class ProductPriceUpdatedConsumer(Carts.API.Data.CartsContext cartsContext) : IConsumer<Catalog.Contracts.ProductPriceUpdated>
{
    public async Task Consume(ConsumeContext<Catalog.Contracts.ProductPriceUpdated> context)
    {
        var message = context.Message;

        await cartsContext.CartItems
            .Where(cartItem => cartItem.ProductId == message.ProductId)
            .ExecuteUpdateAsync(s => s.SetProperty(e => e.Price, e => message.NewPrice), context.CancellationToken);

    }
}

public sealed class ProductDetailsUpdatedConsumer(Carts.API.Data.CartsContext cartsContext) : IConsumer<Catalog.Contracts.ProductDetailsUpdated>
{
    public async Task Consume(ConsumeContext<Catalog.Contracts.ProductDetailsUpdated> context)
    {
        var message = context.Message;

        await cartsContext.CartItems
            .Where(cartItem => cartItem.ProductId == message.ProductId)
            .ExecuteUpdateAsync(s => 
                s.SetProperty(e => e.Name, e => message.Name)
                 .SetProperty(e => e.Description, e => message.Description), context.CancellationToken);

    }
}