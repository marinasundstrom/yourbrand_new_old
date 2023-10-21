namespace Catalog.API.Features.ProductManagement;

public record class CreateProductData(string Name, string Handle, string StoreId, bool HasVariants, string? Description, long? BrandId, long? GroupId, string? Sku, decimal? Price, decimal? RegularPrice, ProductVisibility? Visibility);