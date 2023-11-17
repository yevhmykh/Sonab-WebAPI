using Microsoft.AspNetCore.Mvc;
using Sonab.Core.Constants;
using Sonab.Core.Errors;
using Sonab.WebAPI.Models;

namespace Sonab.WebAPI.Presenters.Users;

public class SubscribePresenter : OkResponsePresenter
{
    public override Task HandleFailure(ErrorBase error)
    {
        Result = error.Problems.First() switch
        {
            UserError.UnauthorizedProblem => new UnauthorizedResult(),
            UserError.AlreadySubscribedProblem => new ConflictObjectResult(new ErrorMessages("Error", Messages.AlreadySubscribed)),
            UserError.NotFoundProblem => new NotFoundResult(),
            _ => new StatusCodeResult(500)
        };
        return Task.CompletedTask;
    }
}
