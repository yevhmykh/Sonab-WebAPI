using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sonab.WebAPI.Models.DB;

namespace Sonab.WebAPI.Contexts.Configurations;

public class MarkTypeConfiguration : BaseTypeConfiguration<Mark>
{
    public override void Configure(EntityTypeBuilder<Mark> builder)
    {
        base.Configure(builder);

        builder
            .HasOne(e => e.User)
            .WithMany(us => us.Marks)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
