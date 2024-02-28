using MediatR;

using Microsoft.AspNetCore.Http.HttpResults;

using YourBrand.Sales.API.Features.OrderManagement.Orders.Commands;
using YourBrand.Sales.API.Features.OrderManagement.Orders.Dtos;
using YourBrand.Sales.API.Features.OrderManagement.Orders.Items;
using YourBrand.Sales.API.Features.OrderManagement.Orders.Items.Commands;
using YourBrand.Sales.API.Features.OrderManagement.Orders.Queries;
using YourBrand.Sales.API.Models;

namespace YourBrand.Sales.API.Features.OrderManagement.Orders;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
        string GetOrdersExpire20 = nameof(GetOrdersExpire20);

        var versionedApi = app.NewVersionedApi("Orders");

        var group = versionedApi.MapGroup("/v{version:apiVersion}/orders")
            .WithTags("Orders")
            .HasApiVersion(ApiVersions.V1)
            .WithOpenApi();

        group.MapGet("/", GetOrders)
            .WithName($"Orders_{nameof(GetOrders)}")
            .CacheOutput(GetOrdersExpire20);

        group.MapGet("/{id}", GetOrderById)
            .WithName($"Orders_{nameof(GetOrderById)}");

        group.MapGet("/getByNo/{orderNo}", GetOrderByNo)
            .WithName($"Orders_{nameof(GetOrderByNo)}");

        group.MapPost("/", CreateOrder)
            .WithName($"Orders_{nameof(CreateOrder)}");

        group.MapPost("/draft", CreateDraftOrder)
            .WithName($"Orders_{nameof(CreateDraftOrder)}");

        group.MapDelete("{id}", DeleteOrder)
            .WithName($"Orders_{nameof(DeleteOrder)}");

        group.MapPost("{id}/items", AddOrderItem)
            .WithName($"Orders_{nameof(AddOrderItem)}");

        group.MapGet("{id}/items/{itemId}", GetOrderItemById)
            .WithName($"Orders_{nameof(GetOrderItemById)}");

        group.MapPut("{id}/items/{itemId}", UpdateOrderItem)
            .WithName($"Orders_{nameof(UpdateOrderItem)}");

        group.MapPut("{id}/status", UpdateStatus)
        .WithName($"Orders_{nameof(UpdateStatus)}");

        group.MapPut("{id}/assignee", UpdateAssignedUser)
            .WithName($"Orders_{nameof(UpdateAssignedUser)}");

        /*

    group.MapPut("{orderId}/items/{id}/price", UpdateOrderItemPrice)
        .WithName($"Orders_{nameof(UpdateOrderItemPrice)}");

    group.MapPut("{orderId}/items/{id}/quantity", UpdateOrderItemQuantity)
        .WithName($"Orders_{nameof(UpdateOrderItemQuantity)}");

    group.MapPut("{orderId}/items/{id}/data", UpdateOrderItemData)
        .WithName($"Orders_{nameof(UpdateOrderItemData)}");

        */

        group.MapDelete("{orderId}/items/{id}", RemoveOrderItem)
            .WithName($"Orders_{nameof(RemoveOrderItem)}");

        return app;
    }

    private static async Task<Ok<PagedResult<OrderDto>>> GetOrders(int[]? status, string? customerId, string? ssn, string? assigneeId, int page = 1, int pageSize = 10, string? sortBy = null, SortDirection? sortDirection = null, IMediator mediator = default!, CancellationToken cancellationToken = default!)
    {
        var result = await mediator.Send(new GetOrders(status, customerId, ssn, assigneeId, page, pageSize, sortBy, sortDirection), cancellationToken);
        return TypedResults.Ok(result.GetValue());
    }

    private static async Task<Results<Ok<OrderDto>, NotFound>> GetOrderById(string id, IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetOrderById(id), cancellationToken);

        if (result.HasError(Errors.Orders.OrderNotFound))
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(result.GetValue());
    }

    private static async Task<Results<Ok<OrderDto>, NotFound>> GetOrderByNo(int orderNo, IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetOrderByNo(orderNo), cancellationToken);

        if (result.HasError(Errors.Orders.OrderNotFound))
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(result.GetValue());
    }

    private static async Task<Results<Created<OrderDto>, NotFound>> CreateOrder(CreateOrderRequest request, IMediator mediator, LinkGenerator linkGenerator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateOrder(request.Status, request.CustomerId, request.BillingDetails, request.ShippingDetails, request.Items), cancellationToken);

        if (result.HasError(Errors.Orders.OrderNotFound))
        {
            return TypedResults.NotFound();
        }

        var order = result.GetValue();

        var path = linkGenerator.GetPathByName(nameof(GetOrderById), new { id = order.Id });

        return TypedResults.Created(path, order);
    }

    private static async Task<Results<Created<OrderDto>, NotFound>> CreateDraftOrder(CreateDraftOrderRequest request, IMediator mediator, LinkGenerator linkGenerator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateDraftOrder(), cancellationToken);

        if (result.HasError(Errors.Orders.OrderNotFound))
        {
            return TypedResults.NotFound();
        }

        var order = result.GetValue();

        var path = linkGenerator.GetPathByName(nameof(GetOrderById), new { id = order.Id });

        return TypedResults.Created(path, order);
    }

    private static async Task<Results<Ok, NotFound>> DeleteOrder(string id, IMediator mediator = default!, LinkGenerator linkGenerator = default!, CancellationToken cancellationToken = default!)
    {
        var result = await mediator.Send(new DeleteOrder(id), cancellationToken);

        if (result.HasError(Errors.Orders.OrderNotFound))
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok();
    }

    private static async Task<Results<Created<OrderItemDto>, NotFound>> AddOrderItem(string id, AddOrderItemRequest request, IMediator mediator = default!, LinkGenerator linkGenerator = default!, CancellationToken cancellationToken = default!)
    {
        var result = await mediator.Send(new CreateOrderItem(id, request.Description, request.ItemId, request.Quantity, request.Unit, request.UnitPrice, request.VatRate, request.Discount, request.Notes), cancellationToken);

        if (result.HasError(Errors.Orders.OrderNotFound))
        {
            return TypedResults.NotFound();
        }

        var orderItem = result.GetValue();

        var path = linkGenerator.GetPathByName(nameof(GetOrderItemById), new { id = orderItem.Id });

        return TypedResults.Created(path, orderItem);
    }

    private static async Task<Results<Created<OrderItemDto>, NotFound>> UpdateOrderItem(string id, string itemId, UpdateOrderItemRequest request, IMediator mediator = default!, LinkGenerator linkGenerator = default!, CancellationToken cancellationToken = default!)
    {
        var result = await mediator.Send(new UpdateOrderItem(id, itemId, request.Description, request.ItemId, request.Quantity, request.Unit, request.UnitPrice, request.VatRate, request.Discount, request.Notes), cancellationToken);

        if (result.HasError(Errors.Orders.OrderNotFound))
        {
            return TypedResults.NotFound();
        }

        var orderItem = result.GetValue();

        var path = linkGenerator.GetPathByName(nameof(GetOrderItemById), new { id = orderItem.Id });

        return TypedResults.Created(path, orderItem);
    }


    private static async Task<Results<Ok<OrderItemDto>, NotFound>> GetOrderItemById(string id, string itemId, IMediator mediator = default!, CancellationToken cancellationToken = default!)
    {
        var result = await mediator.Send(new GetOrderItemById(id, itemId), cancellationToken);

        if (result.HasError(Errors.Orders.OrderNotFound))
        {
            return TypedResults.NotFound();
        }

        if (result.HasError(Errors.Orders.OrderItemNotFound))
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(result.GetValue());
    }

    private static async Task<Results<Ok, NotFound>> UpdateStatus(string id, int status, IMediator mediator = default!, CancellationToken cancellationToken = default!)
    {
        var result = await mediator.Send(new UpdateStatus(id, status), cancellationToken);

        if (result.HasError(Errors.Orders.OrderNotFound))
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok();
    }

    private static async Task<Results<Ok, NotFound>> UpdateAssignedUser(string id, string? userId, IMediator mediator = default!, CancellationToken cancellationToken = default!)
    {
        var result = await mediator.Send(new UpdateAssignedUser(id, userId), cancellationToken);

        if (result.HasError(Errors.Orders.OrderNotFound))
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok();
    }


    /*

    private static async Task<Results<Ok<OrderItemDto>, NotFound>> UpdateOrderItemPrice(string orderId, string id, decimal price, IMediator mediator = default!, LinkGenerator linkGenerator = default!, CancellationToken cancellationToken = default!)
    {
        var result = await mediator.Send(new UpdateOrderItemPrice(orderId, id, price), cancellationToken);

        if (result.HasError(Errors.Orders.OrderNotFound))
        {
            return TypedResults.NotFound();
        }

        if (result.HasError(Errors.Orders.OrderItemNotFound))
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(result.GetValue());
    }

    private static async Task<Results<Ok<OrderItemDto>, NotFound>> UpdateOrderItemQuantity(string orderId, string id, int quantity, IMediator mediator = default!, LinkGenerator linkGenerator = default!, CancellationToken cancellationToken = default!)
    {
        var result = await mediator.Send(new UpdateOrderItemQuantity(orderId, id, quantity), cancellationToken);

        if (result.HasError(Errors.Orders.OrderNotFound))
        {
            return TypedResults.NotFound();
        }

        if (result.HasError(Errors.Orders.OrderItemNotFound))
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(result.GetValue());
    }

    private static async Task<Results<Ok<OrderItemDto>, NotFound>> UpdateOrderItemData(string orderId, string id, string data, IMediator mediator = default!, LinkGenerator linkGenerator = default!, CancellationToken cancellationToken = default!)
    {
        var result = await mediator.Send(new UpdateOrderItemData(orderId, id, data), cancellationToken);

        if (result.HasError(Errors.Orders.OrderNotFound))
        {
            return TypedResults.NotFound();
        }

        if (result.HasError(Errors.Orders.OrderItemNotFound))
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(result.GetValue());
    }

    */

    private static async Task<Results<Ok, NotFound>> RemoveOrderItem(string id, string itemId, IMediator mediator = default!, LinkGenerator linkGenerator = default!, CancellationToken cancellationToken = default!)
    {
        var result = await mediator.Send(new RemoveOrderItem(id, itemId), cancellationToken);

        if (result.HasError(Errors.Orders.OrderNotFound))
        {
            return TypedResults.NotFound();
        }

        if (result.HasError(Errors.Orders.OrderItemNotFound))
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok();
    }
}