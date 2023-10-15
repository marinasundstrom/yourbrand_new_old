using Catalog.API.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Features.ProductManagement.Products.Attributes;

public record DeleteProductAttribute(long ProductId, string AttributeId) : IRequest
{
    public class Handler : IRequestHandler<DeleteProductAttribute>
    {
        private readonly CatalogContext _context;

        public Handler(CatalogContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteProductAttribute request, CancellationToken cancellationToken)
        {
            var item = await _context.Products
                .Include(x => x.ProductAttributes)
                .FirstAsync(x => x.Id == request.ProductId);

            var attribute = item.ProductAttributes
                .First(x => x.AttributeId == request.AttributeId);

            item.ProductAttributes.Remove(attribute);
            _context.ProductAttributes.Remove(attribute);

            await _context.SaveChangesAsync();

        }
    }
}