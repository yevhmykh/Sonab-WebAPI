using Microsoft.AspNetCore.Mvc;
using Sonab.Core.Dto.Posts.Responses;

namespace Sonab.WebAPI.Presenters.Posts;

public class PostPresenter : ApiPresenter<GetPostResponse>
{
    public override Task HandleSuccess(GetPostResponse response)
    {
        Result = new OkObjectResult(response.Post);
        return Task.CompletedTask;
    }
}
