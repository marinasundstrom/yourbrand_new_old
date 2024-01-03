using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.YourService.API.Features.OrderManagement.Orders.Dtos;

using YourBrand.YourService.API.Services;

namespace YourBrand.YourService.API.Features.OrderManagement.Orders.Statuses.Commands;

public record DeleteOrderStatusCommand(int Id) : IRequest
{
    public class DeleteOrderStatusCommandHandler : IRequestHandler<DeleteOrderStatusCommand>
    {
        private readonly IAppDbContext context;

        public DeleteOrderStatusCommandHandler(IAppDbContext context)
        {
            this.context = context;
        }

        public async Task Handle(DeleteOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var orderStatus = await context.OrderStatuses
                .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (orderStatus is null) throw new Exception();

            context.OrderStatuses.Remove(orderStatus);

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}