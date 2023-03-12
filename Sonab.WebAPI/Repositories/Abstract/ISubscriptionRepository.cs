using Sonab.WebAPI.Models.DB;
using Sonab.WebAPI.Models.Subscriptions;

namespace Sonab.WebAPI.Repositories.Abstract;

public interface ISubscriptionRepository
{
    Task<List<SubscriptionFullInfo>> GetByAsync(string externalId);
    Task<UserSubscription> GetSubscriptionAsync(string externalId, int publisherId);
    Task<bool> IsExistsAsync(UserSubscription subscription);
    Task<bool> IsSubscribedAsync(User publisher, string subscriberExternalId);
    Task<bool> AddAndSaveAsync(UserSubscription subscription);
    Task<bool> DeleteAndSaveAsync(UserSubscription subscription);
}
