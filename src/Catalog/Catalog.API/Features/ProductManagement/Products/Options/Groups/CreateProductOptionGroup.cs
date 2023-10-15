using MediatR;

using Microsoft.EntityFrameworkCore;

using Catalog.API.Features.ProductManagement.Options;
using Catalog.API.Persistence;
using Catalog.API.Domain.Entities;

namespace Catalog.API.Features.ProductManagement.Products.Options.Groups;

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
            var item = await _context.Products
                .FirstAsync(x => x.Id == request.ProductId);

            var group = new OptionGroup(Guid.NewGuid().ToString())
            {
                Name = request.Data.Name,
                Description = request.Data.Description,
                Min = request.Data.Min,
                Max = request.Data.Max
            };

            item.OptionGroups.Add(group);

            await _context.SaveChangesAsync();

            return new OptionGroupDto(group.Id, group.Name, group.Description, group.Seq, group.Min, group.Max);
        }
    }
}