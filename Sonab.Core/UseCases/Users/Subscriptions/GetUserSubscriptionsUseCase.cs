using Sonab.Core.Dto.Users.Subscriptions;
using Sonab.Core.Dto.Users.Subscriptions.Requests;
using Sonab.Core.Dto.Users.Subscriptions.Responses;
using Sonab.Core.Interfaces;
using Sonab.Core.Interfaces.Repositories.ReadEntity;

namespace Sonab.Core.UseCases.Users.Subscriptions;

public class GetUserSubscriptionsUseCase : AuthorizedUseCase<GetUserSubscriptionsRequest, GetUserSubscriptionsResponse>
{
    private readonly ISubscriptionRepository _repository;

    public GetUserSubscriptionsUseCase(ISubscriptionRepository repository)
    {
        _repository = repository;
    }

    protected override async Task Handle(
        string userExternalId,
        GetUserSubscriptionsRequest request,
        IPresenter<GetUserSubscriptionsResponse> presenter)
    {
        List<SubscriptionFullInfo> result = await _repository.GetByAsync(userExternalId);
        await presenter.HandleSuccess(new GetUserSubscriptionsResponse(result));
    }
}
