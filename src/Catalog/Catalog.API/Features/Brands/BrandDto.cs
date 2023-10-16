using System.ComponentModel.DataAnnotations;

namespace Catalog.API.Features.Brands;

public record BrandDto
(
    int Id,
    string Name,
    string Handle
);