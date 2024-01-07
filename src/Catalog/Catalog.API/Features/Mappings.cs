using YourBrand.Catalog.API.Domain.Entities;
using YourBrand.Catalog.API.Features.Brands;
using YourBrand.Catalog.API.Features.Currencies;
using YourBrand.Catalog.API.Features.ProductManagement;
using YourBrand.Catalog.API.Features.ProductManagement.Attributes;
using YourBrand.Catalog.API.Features.ProductManagement.Options;
using YourBrand.Catalog.API.Features.ProductManagement.ProductCategories;
using YourBrand.Catalog.API.Features.ProductManagement.Products;
using YourBrand.Catalog.API.Features.Stores;
using YourBrand.Catalog.API.Features.VatRates;

namespace YourBrand.Catalog.API.Features;

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
            product.Parent?.ToParentProductDto(),
            product.Description,
            product.Price,
            product.VatRate,
            product.VatRateId,
            product.RegularPrice,
            product.DiscountRate,
            product.Image?.ToDto(),
            product.Images.Select(x => x.ToDto()),
            product.Handle,
            product.Sku,
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
                item.Image?.ToDto(),
                item.Handle);
    }

    public static ProductImageDto ToDto(this Domain.Entities.ProductImage x)
    {
        return new ProductImageDto(x.Id, x.Title, x.Text, x.Url);
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

    public static VatRateDto ToDto(this Domain.Entities.VatRate vatRate)
    {
        return new(vatRate.Id, vatRate.Name, vatRate.Factor, vatRate.Factor2);
    }
}