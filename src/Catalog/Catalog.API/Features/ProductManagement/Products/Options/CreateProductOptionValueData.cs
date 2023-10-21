namespace Catalog.API.Features.ProductManagement;

public record class CreateProductOptionValueData(string Name, string? SKU, decimal? Price);