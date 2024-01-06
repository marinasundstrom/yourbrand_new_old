using FluentValidation;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using YourBrand.YourService.API.Features.OrderManagement.Orders;
using YourBrand.YourService.API.Infrastructure.Idempotence;

using YourBrand.YourService.API.Behaviors;
using YourBrand.YourService.API.Common;

namespace YourBrand.YourService.API.Features.OrderManagement;

public static class ServiceExtensions
{
    public static IServiceCollection AddOrderManagement(this IServiceCollection services)
    {
        return services;
    }
}