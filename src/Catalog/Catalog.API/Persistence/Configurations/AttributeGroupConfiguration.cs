using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Catalog.API.Domain.Entities;

namespace Catalog.API.Persistence.Configurations;

public class AttributeGroupConfiguration : IEntityTypeConfiguration<AttributeGroup>
{
    public void Configure(EntityTypeBuilder<AttributeGroup> builder)
    {
        builder.ToTable("AttributeGroups");
    }
}
