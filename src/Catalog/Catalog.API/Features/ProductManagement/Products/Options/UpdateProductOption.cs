using YourBrand.Catalog.API.Domain.Entities;
using YourBrand.Catalog.API.Features.ProductManagement.Options;
using YourBrand.Catalog.API.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.API.Features.ProductManagement.Products.Options;

public record UpdateProductOption(long ProductId, string OptionId, UpdateProductOptionData Data) : IRequest<OptionDto>
{
    public class Handler : IRequestHandler<UpdateProductOption, OptionDto>
    {
        private readonly CatalogContext _context;

        public Handler(CatalogContext context)
        {
            _context = context;
        }

        public async Task<OptionDto> Handle(UpdateProductOption request, CancellationToken cancellationToken)
        {
            var product = await _context.Products
            .AsNoTracking()
            .FirstAsync(x => x.Id == request.ProductId);

            var option = await _context.Options
                .Include(x => (x as ChoiceOption)!.Values)
                .Include(x => x.Group)
                .FirstAsync(x => x.Id == request.OptionId);

            var group = await _context.OptionGroups
                .FirstOrDefaultAsync(x => x.Id == request.Data.GroupId);

            option.Name = request.Data.Name;
            option.Description = request.Data.Description;
            option.Group = group;

            if (option.OptionType == Domain.Enums.OptionType.YesOrNo)
            {
                if (option is SelectableOption selectableOption)
                {
                    selectableOption.IsSelected = request.Data.IsSelected.GetValueOrDefault();
                    selectableOption.SKU = request.Data.SKU;
                    selectableOption.Price = request.Data.Price;
                }
            }
            else if (option.OptionType == Domain.Enums.OptionType.NumericalValue)
            {
                if (option is NumericalValueOption numericalValue)
                {
                    numericalValue.MinNumericalValue = request.Data.MinNumericalValue;
                    numericalValue.MaxNumericalValue = request.Data.MaxNumericalValue;
                    numericalValue.DefaultNumericalValue = request.Data.DefaultNumericalValue;
                }
            }
            else if (option.OptionType == Domain.Enums.OptionType.TextValue)
            {
                if (option is TextValueOption textValueOption)
                {
                    textValueOption.TextValueMinLength = request.Data.TextValueMinLength;
                    textValueOption.TextValueMaxLength = request.Data.TextValueMaxLength;
                    textValueOption.DefaultTextValue = request.Data.DefaultTextValue;
                }
            }
            else if (option.OptionType == Domain.Enums.OptionType.Choice)
            {
                if (option is ChoiceOption choiceOption)
                {
                    foreach (var v in request.Data.Values)
                    {
                        if (v.Id == null)
                        {
                            var value = new OptionValue(v.Name)
                            {
                                SKU = v.SKU,
                                Price = v.Price
                            };

                            choiceOption.Values.Add(value);
                            _context.OptionValues.Add(value);
                        }
                        else
                        {
                            var value = choiceOption!.Values.First(x => x.Id == v.Id);

                            value.Name = v.Name;
                            value.SKU = v.SKU;
                            value.Price = v.Price;
                        }
                    }

                    choiceOption!.DefaultValueId = choiceOption!.Values.FirstOrDefault(x => x.Id == request.Data.DefaultOptionValueId)?.Id;

                    foreach (var v in choiceOption!.Values.ToList())
                    {
                        if (_context.Entry(v).State == EntityState.Added)
                            continue;

                        var value = request.Data.Values.FirstOrDefault(x => x.Id == v.Id);

                        if (value is null)
                        {
                            choiceOption!.Values.Remove(v);
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();

            return option.ToDto();
        }
    }
}