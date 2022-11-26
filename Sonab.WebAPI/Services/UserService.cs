using Sonab.WebAPI.Enums;
using Sonab.WebAPI.Models;
using Sonab.WebAPI.Repositories.Abstract;
using Sonab.WebAPI.Services.Abstract;
using Sonab.WebAPI.Utils.Abstact;

namespace Sonab.WebAPI.Services;

public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly IBackgroundQueue _queue;
    private readonly IUserRepository _repository;

    public UserService(
        ILogger<UserService> logger,
        IBackgroundQueue queue,
        IUserRepository repository)
    {
        _logger = logger;
        _queue = queue;
        _repository = repository;
    }

    public async Task<ServiceResponse> IsLoadedAsync(string externalId)
    {
        _logger.LogDebug($"Checking ID: '{externalId}'");
        bool result = await _repository.CheckIdAsync(externalId);

        return ServiceResponse.CreateOk(result);
    }

    public void RequestLoading(string externalId)
    {
        _logger.LogDebug($"Request from new user with ID: '{externalId}'");
        _queue.Enqueue(new JobInfo(JobType.LoadInfo, externalId));
    }
}
