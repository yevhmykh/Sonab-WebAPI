using Sonab.WebAPI.Extentions;
using Sonab.WebAPI.Models;
using Sonab.WebAPI.Models.DB;
using Sonab.WebAPI.Models.Subscriptions;
using Sonab.WebAPI.Repositories.Abstract;
using Sonab.WebAPI.Services.Abstract;

namespace Sonab.WebAPI.Services;

public class SubsriptionService : ISubsriptionService
{
    private readonly ILogger<SubsriptionService> _logger;
    private readonly IHttpContextAccessor _accessor;
    private readonly ISubscriptionRepository _repository;
    private readonly IUserRepository _userRepository;

    public SubsriptionService(
        ILogger<SubsriptionService> logger,
        IHttpContextAccessor accessor,
        ISubscriptionRepository repository,
        IUserRepository userRepository)
    {
        _logger = logger;
        _accessor = accessor;
        _repository = repository;
        _userRepository = userRepository;
    }

    public async Task<ServiceResponse> GetAsync()
    {
        List<SubscriptionFullInfo> result = await _repository
            .GetByAsync(_accessor.GetUserId());
        return ServiceResponse.CreateOk(result);
    }

    public async Task<ServiceResponse> SubscribeAsync(int publisherId)
    {
        User subscriber = await _userRepository.GetByExternalIdAsync(_accessor.GetUserId());
        if (subscriber == null)
        {
            _logger.LogError($"User information is not loaded. ID: '{_accessor.GetUserId()}'");
            return ServiceResponse.CreateConflict(Messages.InfoNotLoaded);
        }

        User publisher = await _userRepository.GetByIdAsync(publisherId);
        if (publisher == null)
        {
            return ServiceResponse.CreateNotFound();
        }

        UserSubscription subscription = new()
        {
            User = subscriber,
            Publisher = publisher
        };
        if (await _repository.IsExistsAsync(subscription))
        {
            return ServiceResponse.CreateConflict(Messages.AlreadySubscribed);
        }

        await _repository.AddAndSaveAsync(subscription);
        return ServiceResponse.CreateOk();
    }

    public async Task<ServiceResponse> UnsubscribeAsync(int publisherId)
    {
        UserSubscription subscription = await _repository.GetSubscriptionAsync(
            _accessor.GetUserId(),
            publisherId);
        if (subscription == null)
        {
            return ServiceResponse.CreateConflict(Messages.NotSubscribed);
        }

        await _repository.DeleteAndSaveAsync(subscription);
        return ServiceResponse.CreateOk();
    }
}
