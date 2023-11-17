using Sonab.Core.BackgroundTasks;
using Sonab.Core.Dto;
using Sonab.Core.Dto.Users;
using Sonab.Core.Interfaces;
using Sonab.Core.Interfaces.Services;

namespace Sonab.Core.UseCases.Users;

public class StartLoadUserInfoUseCase : AuthorizedUseCase<StartLoadUserInfoRequest, OkResponse>
{
    private readonly IBackgroundTaskQueue _taskQueue;

    public StartLoadUserInfoUseCase(IBackgroundTaskQueue taskQueue)
    {
        _taskQueue = taskQueue;
    }

    protected override Task Handle(
        string userExternalId,
        StartLoadUserInfoRequest request,
        IPresenter<OkResponse> presenter)
    {
        _taskQueue.Enqueue(new LoadUserInfoTask(userExternalId));
        return Task.CompletedTask;
    }
}
