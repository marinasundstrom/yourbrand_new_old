using MediatR;

using Microsoft.EntityFrameworkCore;

using Catalog.API.Features.ProductManagement.Attributes;
using Catalog.API.Persistence;
using Catalog.API.Domain.Entities;

namespace Catalog.API.Features.ProductManagement.Products.Variants;

public record GetAvailableAttributeValues(long ProductId, string AttributeId, IDictionary<string, string?> SelectedAttributeValues) : IRequest<IEnumerable<AttributeValueDto>>
{
    public class Handler : IRequestHandler<GetAvailableAttributeValues, IEnumerable<AttributeValueDto>>
    {
        private readonly CatalogContext _context;

        public Handler(CatalogContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AttributeValueDto>> Handle(GetAvailableAttributeValues request, CancellationToken cancellationToken)
        {
            IEnumerable<Product> variants = await _context.Products
                .AsSplitQuery()
                .AsNoTracking()
                .Include(pv => pv.ParentProduct)
                .Include(pv => pv.ProductAttributes)
                .ThenInclude(pv => pv.Attribute)
                .Include(pv => pv.ProductAttributes)
                .ThenInclude(pv => pv.Value)
                .Where(pv => pv.ParentProduct!.Id == request.ProductId)
                .ToArrayAsync();

            foreach (var selectedAttribute in request.SelectedAttributeValues)
            {
                if (selectedAttribute.Value is null)
                    continue;

                variants = variants.Where(x => x.ProductAttributes.Any(vv => vv.Attribute.Id == selectedAttribute.Key && vv.Value?.Id == selectedAttribute.Value));
            }

            var values = variants
                .SelectMany(x => x.ProductAttributes)
                .DistinctBy(x => x.Attribute)
                .Where(x => x.Attribute.Id == request.AttributeId)
                .Select(x => x.Value)
                .Where(x => x is not null);

            return values.Select(x => new AttributeValueDto(x!.Id, x.Name, x.Seq));
        }
    }
}