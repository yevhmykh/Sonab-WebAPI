using Sonab.WebAPI.Enums;
using Sonab.WebAPI.Models.Jobs;
using Sonab.WebAPI.Services.Abstract;
using Sonab.WebAPI.Utils.Abstact;

namespace Sonab.WebAPI.Services;

[Obsolete]
public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly IBackgroundQueue _queue;

    public UserService(
        ILogger<UserService> logger,
        IBackgroundQueue queue)
    {
        _logger = logger;
        _queue = queue;
    }

    public void RequestLoading(string externalId)
    {
        _logger.LogDebug($"Request from new user with ID: '{externalId}'");
        _queue.Enqueue(new JobInfo(JobType.LoadInfo, externalId));
    }
}
