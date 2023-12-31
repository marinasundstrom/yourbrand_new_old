using YourBrand.Catalog.API.Domain.Entities;
using YourBrand.Catalog.API.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.API.Features.ProductManagement.Options;

public record GetOptions(bool IncludeChoices) : IRequest<IEnumerable<OptionDto>>
{
    public class Handler : IRequestHandler<GetOptions, IEnumerable<OptionDto>>
    {
        private readonly CatalogContext _context;

        public Handler(CatalogContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OptionDto>> Handle(GetOptions request, CancellationToken cancellationToken)
        {
            var query = _context.Options
                .AsSplitQuery()
                .AsNoTracking()
                .Include(o => o.Group)
                .Include(o => (o as ChoiceOption)!.Values)
                .Include(o => (o as ChoiceOption)!.DefaultValue)
                .AsQueryable();

            /*
            if(includeChoices)
            {
                query = query.Where(x => !x.Values.Any());
            }
            */

            var options = await query.ToArrayAsync();

            return options.Select(x => x.ToDto());
        }
    }
}