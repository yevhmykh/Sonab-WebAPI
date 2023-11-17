using Microsoft.AspNetCore.Mvc;
using Sonab.Core.Errors;
using Sonab.Core.Interfaces;

namespace Sonab.WebAPI.Presenters;

public abstract class ListApiPresenter<TResponse, TResultItem> : ApiPresenter<TResponse> where TResponse : ResponseDto
{
    public override Task HandleSuccess(TResponse response)
    {
        Result = new OkObjectResult(GetItems(response));
        return Task.CompletedTask;
    }

    protected abstract List<TResultItem> GetItems(TResponse response);

    public override Task HandleFailure(ErrorBase error)
    {
        Result = error is UserError userError && userError.IsUnauthorized()
            ? new UnauthorizedResult()
            : new StatusCodeResult(500);
        return Task.CompletedTask;
    }
}
