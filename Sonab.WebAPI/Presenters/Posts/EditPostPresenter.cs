using Microsoft.AspNetCore.Mvc;
using Sonab.Core.Constants;
using Sonab.Core.Errors;
using Sonab.WebAPI.Models;

namespace Sonab.WebAPI.Presenters.Posts;

public class EditPostPresenter : OkResponsePresenter
{
    private readonly EditRequest _request;

    public EditPostPresenter(EditRequest request)
    {
        _request = request;
    }

    public override Task HandleFailure(ErrorBase error)
    {
        Result = error.Problems.First() switch
        {
            UserError.UnauthorizedProblem => new UnauthorizedResult(),
            PostError.NotFoundProblem => new NotFoundResult(),
            PostError.NotOwnerProblem => new ObjectResult(new ErrorMessages("Error", Messages.OnlyOwner)) {StatusCode = 403},
            PostError.NotFoundTopicsProblem topics => new NotFoundObjectResult(
                new ErrorMessages(Messages.NotFound, _request.GetFieldsByIds(topics.Ids))),
            PostError.MinLenghtViolationProblem problem => new BadRequestObjectResult(
                new ErrorMessages(problem.FieldName, $"Expected min lenght is {problem.Expected}")),
            PostError.MaxLenghtViolationProblem problem => new BadRequestObjectResult(
                new ErrorMessages(problem.FieldName, $"Expected max lenght is {problem.Expected}")),
            _ => new StatusCodeResult(500)
        };
        return Task.CompletedTask;
    }
}
