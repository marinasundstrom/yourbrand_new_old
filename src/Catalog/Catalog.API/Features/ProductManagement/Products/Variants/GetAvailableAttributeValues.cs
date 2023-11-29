using Catalog.API.Domain.Entities;
using Catalog.API.Features.ProductManagement.Attributes;
using Catalog.API.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Features.ProductManagement.Products.Variants;

public record GetAvailableAttributeValues(string ProductIdOrHandle, string AttributeId, IDictionary<string, string?> SelectedAttributeValues) : IRequest<IEnumerable<AttributeValueDto>>
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
            bool isProductId = long.TryParse(request.ProductIdOrHandle, out var productId);

            var query = _context.Products
                .AsSplitQuery()
                .AsNoTracking()
                .Include(pv => pv.ProductAttributes)
                .ThenInclude(pv => pv.Attribute)
                .ThenInclude(pv => pv.Values)
                .Include(pv => pv.ProductAttributes)
                .ThenInclude(pv => pv.Value)
                .AsQueryable();

            query = isProductId ?
                query.Where(pv => pv.ParentProduct!.Id == productId)
                : query.Where(pv => pv.ParentProduct!.Handle == request.ProductIdOrHandle);

            IEnumerable<Product> variants = await query.ToArrayAsync(cancellationToken);

            foreach (var selectedAttribute in request.SelectedAttributeValues)
            {
                if (selectedAttribute.Value is null)
                    continue;

                variants = variants.Where(x => x.ProductAttributes.Any(vv => vv.Attribute.Id == selectedAttribute.Key && vv.Value?.Id == selectedAttribute.Value));
            }

            var values = variants
                .SelectMany(x => x.ProductAttributes)
                .Where(x => x.Attribute.Id == request.AttributeId)
                .Select(x => x.Value)
                .DistinctBy(x => x.Id);

            return values.Select(x => new AttributeValueDto(x!.Id, x.Name, x.Seq));
        }
    }
}