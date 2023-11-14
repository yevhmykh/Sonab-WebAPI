using Sonab.Core.Dto;
using Sonab.Core.Dto.Users.Subscriptions.Requests;
using Sonab.Core.Entities;
using Sonab.Core.Errors;
using Sonab.Core.Interfaces;
using Sonab.Core.Interfaces.Repositories;

namespace Sonab.Core.UseCases.Users.Subscriptions;

public class SubscribeUseCase : AuthorizedUseCase<SubscribeRequest, OkResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<User> _userRepository;

    public SubscribeUseCase(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _userRepository = unitOfWork.GetRepository<User>();
    }

    protected override async Task Handle(
        string userExternalId,
        SubscribeRequest request,
        IPresenter<OkResponse> presenter)
    {
        User user = await _userRepository.GetByAsync(user => user.ExternalId == userExternalId);
        User publisher = await _userRepository.GetByIdAsync(request.PublisherId);
        if (publisher == null)
        {
            await presenter.HandleFailure(UserError.NotFound());
        }

        if (!user.TryAddSubscription(publisher, out UserError error))
        {
            await presenter.HandleFailure(error);
        }

        await _userRepository.UpdateAsync(user);
        await _unitOfWork.Commit();
        await presenter.HandleSuccess(new OkResponse());
    }
}
