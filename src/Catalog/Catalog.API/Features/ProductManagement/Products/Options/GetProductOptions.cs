using MediatR;

using Microsoft.EntityFrameworkCore;

using Catalog.API.Features.ProductManagement.Options;
using Catalog.API.Persistence;
using Catalog.API.Domain.Entities;

namespace Catalog.API.Features.ProductManagement.Products.Options;

public record GetProductOptions(long ProductId, string? VariantId) : IRequest<IEnumerable<ProductOptionDto>>
{
    public class Handler : IRequestHandler<GetProductOptions, IEnumerable<ProductOptionDto>>
    {
        private readonly CatalogContext _context;

        public Handler(CatalogContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductOptionDto>> Handle(GetProductOptions request, CancellationToken cancellationToken)
        {
            var item = await _context.Products
                .AsSplitQuery()
                .AsNoTracking()
                .Include(pv => pv.ProductOptions)
                .ThenInclude(pv => pv.Option.Group)
                .Include(pv => pv.ProductOptions)
                .ThenInclude(pv => (pv.Option as ChoiceOption)!.Values)
                .Include(pv => pv.ProductOptions)
                .ThenInclude(pv => (pv.Option as ChoiceOption)!.DefaultValue)
                .FirstAsync(p => p.Id == request.ProductId);

            var options = item.ProductOptions
                .ToList();

            return options.Select(x => x.ToDto());
        }
    }
}