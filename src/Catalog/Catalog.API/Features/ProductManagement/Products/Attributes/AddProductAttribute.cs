using Catalog.API.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Features.ProductManagement.Products.Attributes;

public record AddProductAttribute(long ProductId, string AttributeId, string ValueId) : IRequest<ProductAttributeDto>
{
    public class Handler : IRequestHandler<AddProductAttribute, ProductAttributeDto>
    {
        private readonly CatalogContext _context;

        public Handler(CatalogContext context)
        {
            _context = context;
        }

        public async Task<ProductAttributeDto> Handle(AddProductAttribute request, CancellationToken cancellationToken)
        {
            var item = await _context.Products
                .FirstAsync(attribute => attribute.Id == request.ProductId, cancellationToken);

            var attribute = await _context.Attributes
                .Include(x => x.Values)
                .FirstOrDefaultAsync(attribute => attribute.Id == request.AttributeId, cancellationToken);

            var value = attribute!.Values
                .First();

            Domain.Entities.ProductAttribute productAttribute = new()
            {
                ProductId = item.Id,
                AttributeId = attribute.Id,
                Value = value!
            };

            item.AddProductAttribute(productAttribute);

            await _context.SaveChangesAsync(cancellationToken);

            return productAttribute.ToDto();
        }
    }
}