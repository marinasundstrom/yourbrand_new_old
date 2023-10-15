namespace Catalog.API.Features.ProductManagement.Options;

public record class OptionGroupDto(string Id, string Name, string? Description, int? Seq, int? Min, int? Max);