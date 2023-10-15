namespace Catalog.API.Features.ProductManagement;

public record class UpdateProductOptionGroupData(string Name, string? Description, int? Min, int? Max);