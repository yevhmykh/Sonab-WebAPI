using Sonab.Core.Dto.Subscriptions;
using Sonab.Core.Entities;

namespace Sonab.Core.Interfaces.Repositories.ReadEntity;

public interface ISubscriptionRepository
{
    Task<List<SubscriptionFullInfo>> GetByAsync(string externalId);
    Task<UserSubscription> GetSubscriptionAsync(string externalId, int publisherId);
    Task<bool> IsExistsAsync(UserSubscription subscription);
    Task<bool> IsSubscribedAsync(User publisher, string subscriberExternalId);
    Task<bool> AddAndSaveAsync(UserSubscription subscription);
    Task<bool> DeleteAndSaveAsync(UserSubscription subscription);
}
