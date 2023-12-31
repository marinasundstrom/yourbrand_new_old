using YourBrand.Catalog.API.Domain.Entities;
using YourBrand.Catalog.API.Features.ProductManagement.Options;
using YourBrand.Catalog.API.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.API.Features.ProductManagement.Products.Options;

public record CreateProductOptionValue(long ProductId, string OptionId, CreateProductOptionValueData Data) : IRequest<OptionValueDto>
{
    public class Handler : IRequestHandler<CreateProductOptionValue, OptionValueDto>
    {
        private readonly CatalogContext _context;

        public Handler(CatalogContext context)
        {
            _context = context;
        }

        public async Task<OptionValueDto> Handle(CreateProductOptionValue request, CancellationToken cancellationToken)
        {
            var product = await _context.Products
            .FirstAsync(x => x.Id == request.ProductId);

            var option = await _context.Options
                .FirstAsync(x => x.Id == request.OptionId);

            var value = new OptionValue(request.Data.Name)
            {
                SKU = request.Data.SKU,
                Price = request.Data.Price
            };

            (option as ChoiceOption)!.Values.Add(value);

            await _context.SaveChangesAsync();

            return new OptionValueDto(value.Id, value.Name, value.SKU, value.Price, value.Seq);
        }
    }
}