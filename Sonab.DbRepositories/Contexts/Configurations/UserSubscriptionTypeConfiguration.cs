using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sonab.Core.Entities;

namespace Sonab.DbRepositories.Contexts.Configurations;

public class UserSubscriptionTypeConfiguration : IEntityTypeConfiguration<UserSubscription>
{
    public void Configure(EntityTypeBuilder<UserSubscription> builder)
    {
        builder.HasKey(us => new { us.UserId, us.PublisherId });
    }
}
