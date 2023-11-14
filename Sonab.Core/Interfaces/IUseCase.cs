using Sonab.Core.Dto;

namespace Sonab.Core.Interfaces;

public interface IUseCase<TRequest, TResponse> where TRequest : RequestDto where TResponse : ResponseDto
{
    Task Handle(LoggedInUser loggedInUser, TRequest request, IPresenter<TResponse> presenter);
}
