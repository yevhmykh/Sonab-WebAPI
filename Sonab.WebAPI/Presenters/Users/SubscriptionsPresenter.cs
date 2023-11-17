using Sonab.Core.Dto.Users.Subscriptions;
using Sonab.Core.Dto.Users.Subscriptions.Responses;

namespace Sonab.WebAPI.Presenters.Users;

public class SubscriptionsPresenter : ListApiPresenter<GetUserSubscriptionsResponse, SubscriptionFullInfo>
{
    protected override List<SubscriptionFullInfo> GetItems(GetUserSubscriptionsResponse response) =>
        response.SubscriptionInfos;
}
