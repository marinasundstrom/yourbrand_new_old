using FluentValidation;

using MediatR;

using YourBrand.Sales.API.Features.OrderManagement.Domain.ValueObjects;
using YourBrand.Sales.API.Features.OrderManagement.Orders.Dtos;
using YourBrand.Sales.API.Features.OrderManagement.Repositories;

namespace YourBrand.Sales.API.Features.OrderManagement.Orders.Commands;

public sealed record UpdateBillingDetails(string Id, BillingDetailsDto BillingDetails) : IRequest<Result>
{
    public sealed class Validator : AbstractValidator<UpdateBillingDetails>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public sealed class Handler(IOrderRepository orderRepository, IUserRepository userRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateBillingDetails, Result>
    {
        private readonly IOrderRepository orderRepository = orderRepository;
        private readonly IUserRepository userRepository = userRepository;
        private readonly IUnitOfWork unitOfWork = unitOfWork;

        public async Task<Result> Handle(UpdateBillingDetails request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.FindByIdAsync(request.Id, cancellationToken);

            if (order is null)
            {
                return Errors.Orders.OrderNotFound;
            }

            var billingDetails = order.BillingDetails ??= new BillingDetails();

            billingDetails.FirstName = request.BillingDetails.FirstName;
            billingDetails.LastName = request.BillingDetails.LastName;
            billingDetails.SSN = request.BillingDetails.SSN;
            billingDetails.Email = request.BillingDetails.Email;
            billingDetails.PhoneNumber = request.BillingDetails.PhoneNumber;
            billingDetails.Address = Map(billingDetails.Address ??= new Address(), request.BillingDetails.Address);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Results.Success;
        }

        private Domain.ValueObjects.Address Map(Domain.ValueObjects.Address a, AddressDto address)
        {
            a.Thoroughfare = address.Thoroughfare;
            a.Premises = address.Premises;
            a.SubPremises = address.SubPremises;
            a.PostalCode = address.PostalCode;
            a.Locality = address.Locality;
            a.SubAdministrativeArea = address.SubAdministrativeArea;
            a.AdministrativeArea = address.AdministrativeArea;
            a.Country = address.Country;

            return a;
        }
    }
}