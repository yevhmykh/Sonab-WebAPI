using Microsoft.AspNetCore.Mvc;
using Sonab.Core.Dto;
using Sonab.Core.Errors;

namespace Sonab.WebAPI.Presenters;

public class OkResponsePresenter : ApiPresenter<OkResponse>
{
    public override Task HandleSuccess(OkResponse response)
    {
        Result = new OkResult();
        return Task.CompletedTask;
    }
}
