namespace BlazorApp.Products;

using System.Collections.Generic;

using BlazorApp.ProductCategories;

using StoreFrontAPI;

public sealed class ProductsService(IProductsClient productsClient) : IProductsService
{
    public async Task<PagedResult<Product>> GetProducts(int? page = 1, int? pageSize = 10, string? searchTerm = null, string? categoryPath = null, CancellationToken cancellationToken = default)
    {
        var results = await productsClient.GetProductsAsync(page, pageSize, searchTerm, categoryPath, cancellationToken);
        return new PagedResult<Product>(results.Items.Select(product => product.Map()), results.Total);
    }

    public async Task<Product> GetProductById(string productIdOrHandle, CancellationToken cancellationToken = default)
    {
        var product = await productsClient.GetProductByIdAsync(productIdOrHandle, cancellationToken);
        return product.Map();
    }

    public async Task<Product> FindProductVariantByAttributes(string productIdOrHandle, Dictionary<string, string?> selectedAttributeValues, CancellationToken cancellationToken = default)
    {
        var product = await productsClient.FindProductVariantByAttributesAsync(productIdOrHandle, selectedAttributeValues, cancellationToken);
        return product?.Map()!;
    }

    public async Task<IEnumerable<Product>> FindProductVariantsByAttributes(string productIdOrHandle, Dictionary<string, string?> selectedAttributeValues, CancellationToken cancellationToken = default)
    {
        var products = await productsClient.FindProductVariantsByAttributesAsync(productIdOrHandle, selectedAttributeValues, cancellationToken);
        return products.Select(x => x.Map());
    }

    public async Task<PagedResult<Product>> GetProductVariants(string productIdOrHandle, int page = 1, int pageSize = 10, string? searchTerm = null, CancellationToken cancellationToken = default)
    {
        var results = await productsClient.GetProductVariantsAsync(productIdOrHandle, page, pageSize, searchTerm, cancellationToken);
        return new PagedResult<Product>(results.Items.Select(x => x.Map()), results.Total);
    }
}

public static class Mapper
{
    public static Product Map(this StoreFrontAPI.Product product)
        => new(product.Id!, product.Name!, product.Category?.ToParentDto2(), product.Image!, product.Description!, product.Price, product.RegularPrice, product.Handle, product.HasVariants, product.Attributes.Select(x => x.Map()), product.Options.Select(x => x.Map()));

    public static ProductAttribute Map(this StoreFrontAPI.ProductAttribute attribute) => new(attribute.Attribute.Map(), attribute.Value?.Map(), attribute.ForVariant, attribute.IsMainAttribute);

    public static Attribute Map(this StoreFrontAPI.Attribute attribute) => new(attribute.Id, attribute.Name, attribute.Description, attribute.Group?.Map(), attribute.Values.Select(x => x.Map()).ToList());

    public static AttributeGroup Map(this StoreFrontAPI.AttributeGroup group) => new(group.Id, group.Name, group.Description);

    public static AttributeValue Map(this StoreFrontAPI.AttributeValue value) => new(value.Id, value.Name, value.Seq);

    public static ProductOption Map(this StoreFrontAPI.ProductOption option) => new(option.Option.Map(), option.IsInherited);

    public static Option Map(this StoreFrontAPI.Option option) => new(option.Id, option.Name, option.Description, (OptionType)option.OptionType, option.Group?.Map(), option.IsRequired, option.Sku, option.Price, option.IsSelected, option.Values.Select(x => x.Map()).ToList(), option.DefaultValue?.Map(), option.MinNumericalValue, option.MaxNumericalValue, option.DefaultNumericalValue, option.TextValueMinLength, option.TextValueMaxLength, option.DefaultTextValue);

    public static OptionGroup Map(this StoreFrontAPI.OptionGroup group) => new(group.Id, group.Name, group.Description, group.Seq, group.Min, group.Max);

    public static OptionValue Map(this StoreFrontAPI.OptionValue value) => new(value.Id, value.Name, value.Sku, value.Price, value.Seq);
}