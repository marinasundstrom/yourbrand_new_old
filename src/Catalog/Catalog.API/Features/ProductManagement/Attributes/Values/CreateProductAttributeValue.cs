using YourBrand.Catalog.API.Domain.Entities;
using YourBrand.Catalog.API.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.API.Features.ProductManagement.Attributes.Values;

public record CreateProductAttributeValue(string Id, CreateProductAttributeValueData Data) : IRequest<AttributeValueDto>
{
    public class Handler : IRequestHandler<CreateProductAttributeValue, AttributeValueDto>
    {
        private readonly CatalogContext _context;

        public Handler(CatalogContext context)
        {
            _context = context;
        }

        public async Task<AttributeValueDto> Handle(CreateProductAttributeValue request, CancellationToken cancellationToken)
        {
            var attribute = await _context.Attributes
                .FirstAsync(x => x.Id == request.Id);

            var value = new AttributeValue(Guid.NewGuid().ToString())
            {
                Name = request.Data.Name
            };

            attribute.Values.Add(value);

            await _context.SaveChangesAsync(cancellationToken);

            return new AttributeValueDto(value.Id, value.Name, value.Seq);
        }
    }
}