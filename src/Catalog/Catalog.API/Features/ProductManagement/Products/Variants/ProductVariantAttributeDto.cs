namespace YourBrand.Catalog.API.Features.ProductManagement;

public record class ProductVariantAttributeDto(string Id, string Name, string? Value, string? ValueId, bool IsMainAttribute);