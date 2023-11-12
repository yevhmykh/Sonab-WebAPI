using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sonab.Core.Entities;

namespace Sonab.WebAPI.Contexts.Configurations;

public class TopicTypeConfiguration : BaseTypeConfiguration<Topic>
{
    public override void Configure(EntityTypeBuilder<Topic> builder)
    {
        base.Configure(builder);

        builder.Property(u => u.Name)
            .IsRequired();
        
        builder
            .HasIndex(e => e.Name)
            .IsUnique();
        
        builder
            .HasIndex(e => e.NormalizedName)
            .IsUnique();
    }
}
