using YourBrand.Catalog.API.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YourBrand.Catalog.API.Persistence.Configurations;

public class AttributeGroupConfiguration : IEntityTypeConfiguration<AttributeGroup>
{
    public void Configure(EntityTypeBuilder<AttributeGroup> builder)
    {
        builder.ToTable("AttributeGroups");
    }
}