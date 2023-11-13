using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sonab.Core.Entities;

namespace Sonab.DbRepositories.Contexts.Configurations;

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
