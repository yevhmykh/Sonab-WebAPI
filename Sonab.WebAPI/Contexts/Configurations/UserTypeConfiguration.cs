using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sonab.WebAPI.Models.DB;

namespace Sonab.WebAPI.Contexts.Configurations;

public class UserTypeConfiguration : BaseTypeConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.Property(u => u.ExternalId)
            .IsRequired();
        
        builder
            .HasIndex(e => e.ExternalId)
            .IsUnique();

        builder.Property(u => u.Email)
            .IsRequired();
        
        builder
            .HasIndex(e => e.Email)
            .IsUnique();

        builder.Property(u => u.Name)
            .IsRequired();

        builder.HasMany(e => e.Subscribers)
            .WithOne(us => us.Publisher)
            .HasForeignKey(us => us.PublisherId);

        builder.HasMany(e => e.Subscriptions)
            .WithOne(us => us.User)
            .HasForeignKey(us => us.UserId);
    }
}
