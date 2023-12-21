﻿using Consul;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Sales.API.Features.OrderManagement.Domain.Entities;
using Sales.API.Features.OrderManagement.Domain.Events;
using Sales.API.Features.OrderManagement.Domain.ValueObjects;
using Sales.API.Features.OrderManagement.Orders.Dtos;
using Sales.API.Features.OrderManagement.Repositories;

namespace Sales.API.Features.OrderManagement.Orders.Commands;

public sealed record CreateOrder(int? Status, string? CustomerId, BillingDetailsDto BillingDetails, ShippingDetailsDto? ShippingDetails, IEnumerable<CreateOrderItemDto> Items) : IRequest<Result<OrderDto>>
{
    public sealed class Validator : AbstractValidator<CreateOrder>
    {
        public Validator()
        {
            //RuleFor(x => x.Title).NotEmpty().MaximumLength(60);

            //RuleFor(x => x.Description).MaximumLength(240);
        }
    }

    public sealed class Handler : IRequestHandler<CreateOrder, Result<OrderDto>>
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

        public async Task<Result<OrderDto>> Handle(CreateOrder request, CancellationToken cancellationToken)
        {
            var order = new Order();

            try
            {
                order.OrderNo = (await orderRepository.GetAll().MaxAsync(x => x.OrderNo)) + 1;
            }
            catch (InvalidOperationException e)
            {
                order.OrderNo = 1; // Order start number
            }

            const int OrderStatusDraft = 1;

            order.StatusId = request.Status ?? OrderStatusDraft;

            order.CustomerId = request.CustomerId;

            order.VatIncluded = true;

            order.BillingDetails = new BillingDetails
            {
                FirstName = request.BillingDetails.FirstName,
                LastName = request.BillingDetails.LastName,
                SSN = request.BillingDetails.SSN,
                Email = request.BillingDetails.Email,
                PhoneNumber = request.BillingDetails.PhoneNumber,
                Address = Map(request.BillingDetails.Address)
            };

            if (request.ShippingDetails is not null)
            {
                order.ShippingDetails = new ShippingDetails
                {
                    FirstName = request.ShippingDetails.FirstName,
                    LastName = request.ShippingDetails.LastName,
                    CareOf = request.ShippingDetails.CareOf,
                    Address = Map(request.ShippingDetails.Address),
                };
            }

            foreach (var orderItem in request.Items)
            {
                order.AddOrderItem(orderItem.Description, orderItem.ItemId, orderItem.Unit, orderItem.UnitPrice, orderItem.VatRate, orderItem.Quantity, orderItem.Notes);
            }

            order.Calculate();

            orderRepository.Add(order);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            /*
            if (request.AssigneeId is not null)
            {
                order.UpdateAssigneeId(request.AssigneeId);

                await unitOfWork.SaveChangesAsync(cancellationToken);

                order.ClearDomainEvents();
            }
            */

            await domainEventDispatcher.Dispatch(new OrderCreated(order.Id), cancellationToken);

            order = await orderRepository.GetAll()
                .Include(i => i.CreatedBy)
                .Include(i => i.LastModifiedBy)
                .FirstAsync(x => x.OrderNo == order.OrderNo, cancellationToken);

            return Result.Success(order!.ToDto());
        }

        private Domain.ValueObjects.Address Map(AddressDto address)
        {
            return new Domain.ValueObjects.Address
            {
                Thoroughfare = address.Thoroughfare,
                Premises = address.Premises,
                SubPremises = address.SubPremises,
                PostalCode = address.PostalCode,
                Locality = address.Locality,
                SubAdministrativeArea = address.SubAdministrativeArea,
                AdministrativeArea = address.AdministrativeArea,
                Country = address.Country
            };
        }
    }
}