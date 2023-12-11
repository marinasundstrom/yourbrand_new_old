using Catalog.API.Domain.Entities;
using Catalog.API.Features.Brands;
using Catalog.API.Features.Currencies;
using Catalog.API.Features.ProductManagement;
using Catalog.API.Features.ProductManagement.Attributes;
using Catalog.API.Features.ProductManagement.Options;
using Catalog.API.Features.ProductManagement.ProductCategories;
using Catalog.API.Features.ProductManagement.Products;
using Catalog.API.Features.Stores;

namespace Catalog.API.Features;

public static class Mappings
{
    public static StoreDto ToDto(this Domain.Entities.Store store)
    {
        return new StoreDto(store.Id, store.Name, store.Handle, store.Currency.ToDto());
    }

    public static BrandDto ToDto(this Domain.Entities.Brand brand)
    {
        return new(brand.Id, brand.Name, brand.Handle);
    }

    public static ProductDto ToDto(this Domain.Entities.Product product)
    {
        return new ProductDto(
            product.Id,
            product.Name,
            product.Store?.ToDto(),
            product.Brand?.ToDto(),
            product.Category?.ToProductCategory2(),
            product.ParentProduct?.ToParentProductDto(),
            product.Description,
            product.Price,
            product.RegularPrice,
            product.Image,
            product.Handle,
            product.SKU,
            product.HasVariants,
            (ProductVisibility)product.Visibility,
            product.ProductAttributes.Select(x => x.ToDto()),
            product.ProductOptions.Select(x => x.ToDto()));
    }

    public static ParentProductDto ToParentProductDto(this Domain.Entities.Product item)
    {
        return new ParentProductDto(
                item.Id,
                item.Name,
                item.Category?.ToDto(),
                item.Description,
                item.Price,
                item.RegularPrice,
                item.Image,
                item.Handle);
    }


    public static ProductAttributeDto ToDto(this Domain.Entities.ProductAttribute x)
    {
        return new ProductAttributeDto(x.Attribute.ToDto(), x.Value?.ToDto(), x.ForVariant, x.IsMainAttribute);
    }

    public static ProductOptionDto ToDto(this Domain.Entities.ProductOption x)
    {
        return new ProductOptionDto(x.Option.ToDto(), x.IsInherited.GetValueOrDefault());
    }

    public static CurrencyDto ToDto(this Domain.Entities.Currency currency)
    {
        return new(currency.Code, currency.Name, currency.Symbol);
    }
}