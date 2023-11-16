using Sonab.Core.Dto;
using Sonab.Core.Errors;
using Sonab.Core.Interfaces;

namespace Sonab.Core.UseCases;

// TODO: Should be more universal, but it's not required now
public abstract class AuthorizedUseCase<TRequest, TResponse> : IUseCase<TRequest, TResponse>
    where TRequest : RequestDto where TResponse : ResponseDto
{
    public Task Handle(LoggedInUser loggedInUser, TRequest request, IPresenter<TResponse> presenter) =>
        string.IsNullOrEmpty(loggedInUser?.ExternalId)
            ? presenter.HandleFailure(UserError.Unauthorized())
            : Handle(loggedInUser.ExternalId.ToUpper(), request, presenter);

    // TODO: Maybe should be User entity or DTO... Yes, need(-_-)
    protected abstract Task Handle(string userExternalId, TRequest request, IPresenter<TResponse> presenter);
}
