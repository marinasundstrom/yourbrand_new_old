using YourBrand.Catalog.API.Domain.Entities;
using YourBrand.Catalog.API.Features.ProductManagement.Options;
using YourBrand.Catalog.API.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.API.Features.ProductManagement.Products.Options.Groups;

public record CreateProductOptionGroup(long ProductId, CreateProductOptionGroupData Data) : IRequest<OptionGroupDto>
{
    public class Handler : IRequestHandler<CreateProductOptionGroup, OptionGroupDto>
    {
        private readonly CatalogContext _context;

        public Handler(CatalogContext context)
        {
            _context = context;
        }

        public async Task<OptionGroupDto> Handle(CreateProductOptionGroup request, CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .FirstAsync(x => x.Id == request.ProductId);

            var group = new OptionGroup(Guid.NewGuid().ToString())
            {
                Name = request.Data.Name,
                Description = request.Data.Description,
                Min = request.Data.Min,
                Max = request.Data.Max
            };

            product.AddOptionGroup(group);

            await _context.SaveChangesAsync();

            return new OptionGroupDto(group.Id, group.Name, group.Description, group.Seq, group.Min, group.Max);
        }
    }
}