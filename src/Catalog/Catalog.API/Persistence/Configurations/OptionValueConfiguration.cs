using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Catalog.API.Domain.Entities;

namespace Catalog.API.Persistence.Configurations;

public class OptionValueConfiguration : IEntityTypeConfiguration<OptionValue>
{
    public void Configure(EntityTypeBuilder<OptionValue> builder)
    {
        builder.ToTable("OptionValues");
    }
}
