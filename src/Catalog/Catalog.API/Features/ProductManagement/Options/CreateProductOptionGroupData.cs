namespace YourBrand.Catalog.API.Features.ProductManagement;

public record class CreateProductOptionGroupData(string Name, string? Description, int? Min, int? Max);