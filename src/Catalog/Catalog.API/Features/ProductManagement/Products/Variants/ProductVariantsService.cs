using Catalog.API.Domain.Entities;
using Catalog.API.Persistence;

using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Features.ProductManagement.Products.Variants;

public class ProductVariantsService
{
    private readonly CatalogContext _context;

    public ProductVariantsService(CatalogContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> FindVariants(string productIdOrHandle, string? itemVariantIdOrHandle, IDictionary<string, string?> selectedAttributeValues, CancellationToken cancellationToken)
    {
        bool isProductId = long.TryParse(productIdOrHandle, out var productId);

        var query = _context.Products
            .AsSplitQuery()
            .AsNoTracking()
            .IncludeAll()
            .AsQueryable()
            .TagWith(nameof(FindVariants));

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
            .ToArrayAsync(cancellationToken);

        foreach (var selectedOption in selectedAttributeValues)
        {
            if (selectedOption.Value is null)
                continue;

            variants = variants.Where(x => x.ProductAttributes.Any(vv => vv.Attribute.Id == selectedOption.Key && vv.Value?.Id == selectedOption.Value));
        }

        return variants;
    }

    public async Task<IEnumerable<AttributeValue>> GetAvailableAttributeValues(string productIdOrHandle, string attributeId, IDictionary<string, string?> selectedAttributeValues, CancellationToken cancellationToken)
    {
        bool isProductId = long.TryParse(productIdOrHandle, out var productId);

        var query = _context.Products
            .AsSplitQuery()
            .AsNoTracking()
            .Include(pv => pv.ProductAttributes)
            .ThenInclude(pv => pv.Attribute)
            .ThenInclude(pv => pv.Values)
            .Include(pv => pv.ProductAttributes)
            .ThenInclude(pv => pv.Value)
            .AsQueryable();

        query = isProductId ?
            query.Where(pv => pv.ParentProduct!.Id == productId)
            : query.Where(pv => pv.ParentProduct!.Handle == productIdOrHandle);

        IEnumerable<Product> variants = await query.ToArrayAsync(cancellationToken);

        foreach (var selectedAttribute in selectedAttributeValues)
        {
            if (selectedAttribute.Value is null)
                continue;

            variants = variants.Where(x => x.ProductAttributes.Any(vv => vv.Attribute.Id == selectedAttribute.Key && vv.Value?.Id == selectedAttribute.Value));
        }

        return variants
            .SelectMany(x => x.ProductAttributes)
            .Where(x => x.Attribute.Id == attributeId)
            .Select(x => x.Value!)
            .DistinctBy(x => x.Id);
    }
}