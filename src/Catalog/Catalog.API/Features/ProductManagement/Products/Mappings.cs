using Catalog.API.Features.ProductManagement.Attributes;
using Catalog.API.Features.ProductManagement.Options;
using Catalog.API.Features.ProductManagement.ProductCategories;

namespace Catalog.API.Features.ProductManagement.Products;

public static class Mappings
{
    public static ProductDto ToDto(this Domain.Entities.Product product)
    {
        return new(product.Id, product.Name, product.Category.ToShortDto(), product.Description, product.Price, product.RegularPrice, product.Image, product.Handle);
    }

    public static ProductAttributeDto ToDto(this Domain.Entities.ProductAttribute x)
    {
        return new ProductAttributeDto(x.Attribute.ToDto(), x.Value?.ToDto(), x.ForVariant, x.IsMainAttribute);
    }

    public static ProductOptionDto ToDto(this Domain.Entities.ProductOption x)
    {
        return new ProductOptionDto(x.Option.ToDto(), x.IsInherited.GetValueOrDefault());
    }

    /*

    public static BrandDto ToDto(this Domain.Entities.Brand brand)
    {
        return new BrandDto(brand.Id, brand.Name, brand.Handle);
    }

    public static CurrencyDto ToDto(this Domain.Entities.Currency currency)
    {
        return new (currency.Code, currency.Name, currency.Symbol);
    }

    */
}