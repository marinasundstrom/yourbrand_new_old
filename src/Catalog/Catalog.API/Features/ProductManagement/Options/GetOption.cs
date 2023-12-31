using YourBrand.Catalog.API.Domain.Entities;
using YourBrand.Catalog.API.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.API.Features.ProductManagement.Options;

public record GetOption(string OptionId) : IRequest<OptionDto>
{
    public class Handler : IRequestHandler<GetOption, OptionDto>
    {
        private readonly CatalogContext _context;

        public Handler(CatalogContext context)
        {
            _context = context;
        }

        public async Task<OptionDto> Handle(GetOption request, CancellationToken cancellationToken)
        {
            var option = await _context.Options
                .AsSplitQuery()
                .AsNoTracking()
                .Include(pv => pv.Group)
                .Include(pv => (pv as ChoiceOption)!.Values)
                .Include(pv => (pv as ChoiceOption)!.DefaultValue)
                .FirstAsync(o => o.Id == request.OptionId);

            return option.ToDto();
        }
    }
}