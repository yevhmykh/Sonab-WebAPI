using Microsoft.EntityFrameworkCore;
using Sonab.WebAPI.Contexts;
using Sonab.WebAPI.Models.DB;
using Sonab.WebAPI.Models.Subscriptions;
using Sonab.WebAPI.Repositories.Abstract;

namespace Sonab.WebAPI.Repositories;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly AppDbContext _context;

    public SubscriptionRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<SubscriptionFullInfo[]> GetByExternalIdAsync(string externalId) =>
        _context.Subscriptions
            .Include(x => x.Publisher)
            .Where(x => x.User.ExternalId == externalId)
            .Select(x => new SubscriptionFullInfo
            {
                PublisherId = x.PublisherId,
                PublisherName = x.Publisher.Name
            })
            .ToArrayAsync();

    public Task<UserSubscription> GetSubscriptionAsync(string externalId, int publisherId) =>
        _context.Subscriptions.FirstOrDefaultAsync(x => x.User.ExternalId == externalId
            && x.PublisherId == publisherId);

    public Task<bool> IsExistsAsync(UserSubscription subscription) =>
        _context.Subscriptions.AnyAsync(x => x.User == subscription.User
            && x.Publisher == subscription.Publisher);

    public Task<bool> IsSubscribedAsync(User publisher, string subscriberExternalId) =>
        _context.Subscriptions.AnyAsync(x => x.Publisher == publisher
            && x.User.ExternalId == subscriberExternalId);

    public async Task<bool> AddAndSaveAsync(UserSubscription subscription)
    {
        await _context.Subscriptions.AddAsync(subscription);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAndSaveAsync(UserSubscription subscription)
    {
        _context.Subscriptions.Remove(subscription);
        return await _context.SaveChangesAsync() > 0;
    }
}
