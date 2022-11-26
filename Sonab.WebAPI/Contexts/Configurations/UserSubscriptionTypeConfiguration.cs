using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sonab.WebAPI.Models.DB;

namespace Sonab.WebAPI.Contexts.Configurations;

public class UserSubscriptionTypeConfiguration : IEntityTypeConfiguration<UserSubscription>
{
    public void Configure(EntityTypeBuilder<UserSubscription> builder)
    {
        builder.HasKey(us => new { us.UserId, us.PublisherId});
    }
}
