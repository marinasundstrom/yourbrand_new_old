using Catalog.API.Features.ProductManagement.Options;
using Catalog.API.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Features.ProductManagement.Products.Options.Groups;

public record GetProductOptionGroups(long ProductId) : IRequest<IEnumerable<OptionGroupDto>>
{
    public class Handler : IRequestHandler<GetProductOptionGroups, IEnumerable<OptionGroupDto>>
    {
        private readonly CatalogContext _context;

        public Handler(CatalogContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OptionGroupDto>> Handle(GetProductOptionGroups request, CancellationToken cancellationToken)
        {
            var groups = await _context.OptionGroups
            .AsTracking()
            .Include(x => x.Product)
            .Where(x => x.Product!.Id == request.ProductId)
            .ToListAsync();

            return groups.Select(group => new OptionGroupDto(group.Id, group.Name, group.Description, group.Seq, group.Min, group.Max));
        }
    }
}