using Microsoft.AspNetCore.Mvc;
using Sonab.Core.Constants;
using Sonab.Core.Errors;
using Sonab.WebAPI.Models;

namespace Sonab.WebAPI.Presenters.Posts;

public class RemovePostPresenter : OkResponsePresenter
{
    public override Task HandleFailure(ErrorBase error)
    {
        Result = error.Problems.First() switch
        {
            UserError.UnauthorizedProblem => new UnauthorizedResult(),
            PostError.NotFoundProblem => new NotFoundResult(),
            PostError.NotOwnerProblem => new ObjectResult(new ErrorMessages("Error", Messages.OnlyOwner)) {StatusCode = 403},
            _ => new StatusCodeResult(500)
        };
        return Task.CompletedTask;
    }
}
