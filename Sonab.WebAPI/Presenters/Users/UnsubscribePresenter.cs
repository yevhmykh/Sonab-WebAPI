using Microsoft.AspNetCore.Mvc;
using Sonab.Core.Constants;
using Sonab.Core.Errors;
using Sonab.WebAPI.Models;

namespace Sonab.WebAPI.Presenters.Users;

public class UnsubscribePresenter : OkResponsePresenter
{
    public override Task HandleFailure(ErrorBase error)
    {
        Result = error.Problems.First() switch
        {
            UserError.UnauthorizedProblem => new UnauthorizedResult(),
            UserError.NotSubscribedProblem => new ConflictObjectResult(
                new ErrorMessages("Error", Messages.NotSubscribed)), // TODO: make NotFoundResult and update UI
            _ => new StatusCodeResult(500)
        };
        return Task.CompletedTask;
    }
}
