using Microsoft.AspNetCore.Mvc;
using Sonab.Core.Errors;
using Sonab.Core.Interfaces;

namespace Sonab.WebAPI.Presenters;

public abstract class ApiPresenter<TResponse> : IPresenter<TResponse> where TResponse : ResponseDto
{
    public IActionResult Result { get; protected set; }
    public abstract Task HandleSuccess(TResponse response);

    public virtual Task HandleFailure(ErrorBase error)
    {
        Result = error is UserError userError && userError.IsUnauthorized()
            ? new UnauthorizedResult()
            : new StatusCodeResult(500);
        return Task.CompletedTask;
    }
}
