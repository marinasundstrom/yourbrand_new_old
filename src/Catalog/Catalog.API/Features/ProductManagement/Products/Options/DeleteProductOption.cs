using Catalog.API.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Features.ProductManagement.Products.Options;

public record DeleteProductOption(long ProductId, string OptionId) : IRequest
{
    public class Handler : IRequestHandler<DeleteProductOption>
    {
        private readonly CatalogContext _context;

        public Handler(CatalogContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteProductOption request, CancellationToken cancellationToken)
        {
            var item = await _context.Products
                .Include(x => x.Options)
                .FirstAsync(x => x.Id == request.ProductId);

            var option = item.Options
                .First(x => x.Id == request.OptionId);

            item.RemoveOption(option);
            _context.Options.Remove(option);

            if (item.HasVariants)
            {
                var variants = await _context.Products
                    .Where(x => x.ParentProductId == item.Id)
                    .Include(x => x.ProductOptions.Where(z => z.OptionId == option.Id))
                    .ToArrayAsync(cancellationToken);

                foreach (var variant in item.Variants)
                {
                    var option1 = variant.ProductOptions.FirstOrDefault(x => x.OptionId == option.Id);
                    if (option1 is not null)
                    {
                        variant.RemoveProductOption(option1);
                    }
                }
            }

            await _context.SaveChangesAsync();

        }
    }
}