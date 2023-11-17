using Microsoft.AspNetCore.Mvc;
using Sonab.Core.Dto;
using Sonab.Core.Interfaces;
using Sonab.WebAPI.Extensions;
using Sonab.WebAPI.Presenters;

namespace Sonab.WebAPI.Controllers;

public abstract class BaseController : ControllerBase
{
    protected async Task<IActionResult> ExecuteUseCase<TRequest, TResponse>(
        TRequest request,
        IUseCase<TRequest, TResponse> useCase,
        ApiPresenter<TResponse> presenter) where TRequest : RequestDto where TResponse : ResponseDto
    {
        User.TryGetUserId(out string userId);
        await useCase.Handle(new LoggedInUser(userId), request, presenter);

        return presenter.Result;
    }
}
