using Microsoft.AspNetCore.Mvc;
using Sonab.Core.Dto.Posts.Responses;

namespace Sonab.WebAPI.Presenters.Posts;

public class PostCountPresenter : ApiPresenter<GetPostCountResponse>
{
    public override Task HandleSuccess(GetPostCountResponse response)
    {
        Result = new OkObjectResult(response.Count);
        return Task.CompletedTask;
    }
}