using Carts.Contracts;

using MassTransit;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Carts.API.Features.CartsManagement.Consumers;

public sealed class GetCartsConsumer(MediatR.IMediator mediator) : IConsumer<Carts.Contracts.GetCarts>
{
    public async Task Consume(ConsumeContext<Carts.Contracts.GetCarts> context)
    {
        var page = context.Message.Page;
        var pageSize = context.Message.PageSize;

        var r = await mediator.Send(new Requests.GetCarts(page, pageSize), context.CancellationToken);
        var cartsResult = r.GetValue();

        var result = new PagedCartResult
        {
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

public sealed class GetCartByTagConsumer(MediatR.IMediator mediator) : IConsumer<GetCartByTag>
{
    public async Task Consume(ConsumeContext<Carts.Contracts.GetCartByTag> context)
    {
        var id = context.Message.Tag;

        var r = await mediator.Send(new Requests.GetCartByTag(id), context.CancellationToken);
        var cart = r.GetValue();

        await context.RespondAsync<GetCartByTagResponse>(new GetCartByTagResponse { Cart = cart.Map() });
    }
}

public sealed class CreateCartConsumer(MediatR.IMediator mediator) : IConsumer<Carts.Contracts.CreateCart>
{
    public async Task Consume(ConsumeContext<Carts.Contracts.CreateCart> context)
    {
        var request = context.Message;

        var r = await mediator.Send(new Requests.CreateCart(request.Tag), context.CancellationToken);

        var cart = r.GetValue();

        await context.RespondAsync<CreateCartResponse>(new CreateCartResponse { Cart = cart.Map() });
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
            request.ProductHandle,
            request.Description,
            request.Price,
            request.RegularPrice,
            request.Quantity,
            request.Data
        ), context.CancellationToken);

        var cartItem = r.GetValue();

        await context.RespondAsync<AddCartItemResponse>(new AddCartItemResponse { CartItem = cartItem.Map() });
    }
}

public sealed class UpdateCartItemPriceConsumer(MediatR.IMediator mediator) : IConsumer<UpdateCartItemPrice>
{
    public async Task Consume(ConsumeContext<Carts.Contracts.UpdateCartItemPrice> context)
    {
        var cartId = context.Message.CartId;
        var cartItemId = context.Message.CartItemId;
        var price = context.Message.Price;

        var r = await mediator.Send(new Requests.UpdateCartItemPrice(cartId, cartItemId, price), context.CancellationToken);
        var cartItem = r.GetValue();

        await context.RespondAsync<UpdateCartItemPriceResponse>(new UpdateCartItemPriceResponse { CartItem = cartItem.Map() });
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

public sealed class UpdateCartItemDataConsumer(MediatR.IMediator mediator) : IConsumer<UpdateCartItemData>
{
    public async Task Consume(ConsumeContext<Carts.Contracts.UpdateCartItemData> context)
    {
        var cartId = context.Message.CartId;
        var cartItemId = context.Message.CartItemId;
        var data = context.Message.Data;

        var r = await mediator.Send(new Requests.UpdateCartItemData(cartId, cartItemId, data), context.CancellationToken);
        var cartItem = r.GetValue();

        await context.RespondAsync<UpdateCartItemDataResponse>(new UpdateCartItemDataResponse { CartItem = cartItem.Map() });
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
    public static Carts.Contracts.Cart Map(this Carts.API.Domain.Entities.Cart cart) => new Carts.Contracts.Cart
    {
        Id = cart.Id,
        Tag = cart.Tag,
        Total = cart.Total,
        Items = cart.Items.Select(cartItem => cartItem.Map())
    };

    public static Carts.Contracts.CartItem Map(this Carts.API.Domain.Entities.CartItem cartItem) => new Carts.Contracts.CartItem
    {
        Id = cartItem.Id,
        Name = cartItem.Name,
        Image = cartItem.Image,
        ProductId = cartItem.ProductId,
        ProductHandle = cartItem.ProductHandle,
        Description = cartItem.Description,
        Price = cartItem.Price,
        RegularPrice = cartItem.RegularPrice,
        Quantity = cartItem.Quantity,
        Total = cartItem.Total,
        Data = cartItem.Data,
        Created = cartItem.Created
    };
}

public sealed class ProductPriceUpdatedConsumer(Carts.API.Persistence.CartsContext cartsContext) : IConsumer<Catalog.Contracts.ProductPriceUpdated>
{
    public async Task Consume(ConsumeContext<Catalog.Contracts.ProductPriceUpdated> context)
    {
        var message = context.Message;

        await cartsContext.CartItems
            .Where(cartItem => cartItem.ProductId == message.ProductId)
            .ExecuteUpdateAsync(s =>
                s.SetProperty(e => e.Price, e => message.NewPrice)
                .SetProperty(e => e.RegularPrice, e => message.RegularPrice), context.CancellationToken);

    }
}

public sealed class ProductDetailsUpdatedConsumer(Carts.API.Persistence.CartsContext cartsContext) : IConsumer<Catalog.Contracts.ProductDetailsUpdated>
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

public sealed class ProductImageUpdatedConsumer(Carts.API.Persistence.CartsContext cartsContext) : IConsumer<Catalog.Contracts.ProductImageUpdated>
{
    public async Task Consume(ConsumeContext<Catalog.Contracts.ProductImageUpdated> context)
    {
        var message = context.Message;

        await cartsContext.CartItems
            .Where(cartItem => cartItem.ProductId == message.ProductId)
            .ExecuteUpdateAsync(s => s.SetProperty(e => e.Image, e => message.ImageUrl), context.CancellationToken);
    }
}


public sealed class ProductHandleUpdatedConsumer(Carts.API.Persistence.CartsContext cartsContext) : IConsumer<Catalog.Contracts.ProductHandleUpdated>
{
    public async Task Consume(ConsumeContext<Catalog.Contracts.ProductHandleUpdated> context)
    {
        var message = context.Message;

        await cartsContext.CartItems
            .Where(cartItem => cartItem.ProductId == message.ProductId)
            .ExecuteUpdateAsync(s => s.SetProperty(e => e.ProductHandle, e => message.Handle), context.CancellationToken);
    }
}