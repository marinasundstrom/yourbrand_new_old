using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Sales.API.Features.OrderManagement.Orders.Dtos;

namespace Sales.API.Features.OrderManagement.Orders.Statuses.Queries;

public record GetOrderStatusQuery(int Id) : IRequest<OrderStatusDto?>
{
    class GetOrderStatusQueryHandler : IRequestHandler<GetOrderStatusQuery, OrderStatusDto?>
    {
        private readonly ISalesContext _context;
        private readonly ICurrentUserService currentUserService;

        public GetOrderStatusQueryHandler(
            ISalesContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            this.currentUserService = currentUserService;
        }

        public async Task<OrderStatusDto?> Handle(GetOrderStatusQuery request, CancellationToken cancellationToken)
        {
            var orderStatus = await _context
               .OrderStatuses
               .AsNoTracking()
               .FirstAsync(c => c.Id == request.Id);

            if (orderStatus is null)
            {
                return null;
            }

            return orderStatus.ToDto();
        }
    }
}