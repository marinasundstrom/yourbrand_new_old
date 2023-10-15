namespace Catalog.API.Features.ProductManagement;

public record class UpdateProductVariantAttributeData(int? Id, string AttributeId, string ValueId);