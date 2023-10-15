using Catalog.API.Features.ProductManagement.Options;
using Catalog.API.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Features.ProductManagement.Products.Options.Groups;

public record UpdateProductOptionGroup(long ProductId, string OptionGroupId, UpdateProductOptionGroupData Data) : IRequest<OptionGroupDto>
{
    public class Handler : IRequestHandler<UpdateProductOptionGroup, OptionGroupDto>
    {
        private readonly CatalogContext _context;

        public Handler(CatalogContext context)
        {
            _context = context;
        }

        public async Task<OptionGroupDto> Handle(UpdateProductOptionGroup request, CancellationToken cancellationToken)
        {
            var item = await _context.Products
            .Include(x => x.OptionGroups)
            .FirstAsync(x => x.Id == request.ProductId);

            var optionGroup = item.OptionGroups
                .First(x => x.Id == request.OptionGroupId);

            optionGroup.Name = request.Data.Name;
            optionGroup.Description = request.Data.Description;
            optionGroup.Min = request.Data.Min;
            optionGroup.Max = request.Data.Max;

            await _context.SaveChangesAsync();

            return new OptionGroupDto(optionGroup.Id, optionGroup.Name, optionGroup.Description, optionGroup.Seq, optionGroup.Min, optionGroup.Max);
        }
    }
}