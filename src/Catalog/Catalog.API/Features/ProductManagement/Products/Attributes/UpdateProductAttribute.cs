using Catalog.API.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Features.ProductManagement.Products.Attributes;

public record UpdateProductAttribute(long ProductId, string AttributeId, string ValueId) : IRequest<ProductAttributeDto>
{
    public class Handler : IRequestHandler<UpdateProductAttribute, ProductAttributeDto>
    {
        private readonly CatalogContext _context;

        public Handler(CatalogContext context)
        {
            _context = context;
        }

        public async Task<ProductAttributeDto> Handle(UpdateProductAttribute request, CancellationToken cancellationToken)
        {
            var item = await _context.Products
                            .AsSplitQuery()
                            .Include(x => x.ProductAttributes)
                            .ThenInclude(x => x.Attribute)
                            .ThenInclude(x => x.Values)
                            .FirstAsync(attribute => attribute.Id == request.ProductId, cancellationToken);

            var productAttribute = item.ProductAttributes
                .First(attribute => attribute.AttributeId == request.AttributeId);

            var value = productAttribute.Attribute.Values
                .First(x => x.Id == request.ValueId);

            productAttribute.ProductId = item.Id;
            productAttribute.Value = value;

            item.ProductAttributes.Add(productAttribute);

            await _context.SaveChangesAsync(cancellationToken);


            return productAttribute.ToDto();
        }
    }
}