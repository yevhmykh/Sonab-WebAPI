using Microsoft.EntityFrameworkCore;
using Sonab.Core.Dto.Users.Subscriptions;
using Sonab.Core.Entities;
using Sonab.Core.Interfaces.Repositories.ReadEntity;
using Sonab.DbRepositories.Contexts;

namespace Sonab.DbRepositories.ReadEntityRepositories;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly AppDbContext _context;

    public SubscriptionRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<List<SubscriptionFullInfo>> GetByAsync(string externalId) =>
        _context.Subscriptions
            .Include(x => x.Publisher)
            .Where(x => x.User.ExternalId == externalId)
            .Select(x => new SubscriptionFullInfo
            {
                PublisherId = x.PublisherId,
                PublisherName = x.Publisher.Name
            })
            .ToListAsync();

    public Task<bool> IsSubscribedAsync(User publisher, string subscriberExternalId) =>
        _context.Subscriptions.AnyAsync(x => x.Publisher == publisher
            && x.User.ExternalId == subscriberExternalId);
}
