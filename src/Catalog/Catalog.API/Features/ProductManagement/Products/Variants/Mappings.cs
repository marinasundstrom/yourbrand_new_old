using Catalog.API.Domain.Entities;

namespace Catalog.API.Features.ProductManagement.Products.Variants;

public static class Mappings
{
    public static ProductVariantAttributeDto ToDto(this ProductAttribute x)
    {
        return new ProductVariantAttributeDto(x.Attribute.Id, x.Attribute.Name, x.Value?.Name, x.Value?.Id, x.IsMainAttribute);
    }
}