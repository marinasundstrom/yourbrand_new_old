using Catalog.API.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.API.Persistence.Configurations;

public class ProductGroupAttributeConfiguration : IEntityTypeConfiguration<ProductCategoryAttribute>
{
    public void Configure(EntityTypeBuilder<ProductCategoryAttribute> builder)
    {
        builder.ToTable("ProductCategoryAttributes");
    }
}
