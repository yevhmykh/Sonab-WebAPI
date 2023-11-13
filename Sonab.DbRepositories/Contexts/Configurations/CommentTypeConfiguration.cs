using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sonab.Core.Entities;

namespace Sonab.DbRepositories.Contexts.Configurations;

public class CommentTypeConfiguration : BaseTypeConfiguration<Comment>
{
    public override void Configure(EntityTypeBuilder<Comment> builder)
    {
        base.Configure(builder);

        builder
            .Property(u => u.Content)
            .IsRequired();

        builder
            .Property(e => e.Created)
            .HasDefaultValueSql("DATETIME('now')");

        builder
            .HasOne(e => e.User)
            .WithMany(us => us.Comments)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
