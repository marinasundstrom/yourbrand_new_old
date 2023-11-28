using Catalog.API.Domain.Entities;
using Catalog.API.Persistence;

using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Features.ProductManagement.Products.Variants;

public class ProductsService
{
    private readonly CatalogContext _context;

    public ProductsService(CatalogContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> FindVariantCore(string productIdOrHandle, string? itemVariantIdOrHandle, IDictionary<string, string?> selectedAttributeValues)
    {
        bool isProductId = long.TryParse(productIdOrHandle, out var productId);

        var query = _context.Products
            .AsSplitQuery()
            .AsNoTracking()
            .IncludeAll()
            .AsQueryable()
            .TagWith(nameof(FindVariantCore));

        query = isProductId ?
            query.Where(pv => pv.ParentProduct!.Id == productId)
            : query.Where(pv => pv.ParentProduct!.Handle == productIdOrHandle);

        if (itemVariantIdOrHandle is not null)
        {
            bool isItemVariantId = long.TryParse(itemVariantIdOrHandle, out var itemVariantId);

            query = isItemVariantId ?
                query.Where(pv => pv.Id == itemVariantId)
                : query.Where(pv => pv.Handle == itemVariantIdOrHandle);
        }

        IEnumerable<Product> variants = await query
            .ToArrayAsync();

        foreach (var selectedOption in selectedAttributeValues)
        {
            if (selectedOption.Value is null)
                continue;

            variants = variants.Where(x => x.ProductAttributes.Any(vv => vv.Attribute.Id == selectedOption.Key && vv.Value?.Id == selectedOption.Value));
        }

        return variants;
    }
}