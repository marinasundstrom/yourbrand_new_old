using FluentValidation;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using YourBrand.YourService.API.Features.OrderManagement;
using YourBrand.YourService.API.Features.OrderManagement.Orders;
using YourBrand.YourService.API.Infrastructure.Idempotence;

using YourBrand.YourService.API.Behaviors;
using YourBrand.YourService.API.Common;

namespace YourBrand.YourService.API;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(x =>
        {
            x.RegisterServicesFromAssemblyContaining(typeof(ServiceExtensions));

            //x.NotificationPublisherType = typeof(IdempotentDomainEventPublisher);
        });

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddValidatorsFromAssembly(typeof(ServiceExtensions).Assembly);

        services.AddOrderManagement();

        return services;
    }
}