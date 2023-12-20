using FluentValidation;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using Sales.API.Features.OrderManagement.Orders;
using Sales.API.Infrastructure.Idempotence;

using YourBrand.Orders.Application.Behaviors;
using YourBrand.Orders.Application.Common;

namespace Sales.API.Features.OrderManagement;

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

        return services;
    }

    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllersForApp();

        services.AddScoped<IOrderNotificationService, OrderNotificationService>();

        return services;
    }

    public static IServiceCollection AddControllersForApp(this IServiceCollection services)
    {
        /*
        var assembly = typeof(OrdersController).Assembly;

        services.AddControllers()
            .AddApplicationPart(assembly);
            */

        return services;
    }
}