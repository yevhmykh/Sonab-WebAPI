using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sonab.WebAPI.Models.DB;

namespace Sonab.WebAPI.Contexts.Configurations;

public class PostTypeConfiguration : BaseTypeConfiguration<Post>
{
    public override void Configure(EntityTypeBuilder<Post> builder)
    {
        base.Configure(builder);

        builder.Property(u => u.Title)
            .IsRequired();
        
        builder
            .HasIndex(e => e.Title)
            .IsUnique();

        builder.Property(u => u.Content)
            .IsRequired();

        builder
            .Property(e => e.Created)
            .HasDefaultValueSql("DATETIME('now')");
    }
}
