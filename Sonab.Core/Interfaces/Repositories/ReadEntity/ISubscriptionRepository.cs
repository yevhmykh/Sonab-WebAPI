using Sonab.Core.Dto.Users.Subscriptions;
using Sonab.Core.Entities;

namespace Sonab.Core.Interfaces.Repositories.ReadEntity;

public interface ISubscriptionRepository
{
    Task<List<SubscriptionFullInfo>> GetByAsync(string externalId);
    Task<bool> IsSubscribedAsync(User publisher, string subscriberExternalId);
}
