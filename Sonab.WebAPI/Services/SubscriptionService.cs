using Sonab.Core.Entities;
using Sonab.WebAPI.Extensions;
using Sonab.WebAPI.Models;
using Sonab.WebAPI.Models.Subscriptions;
using Sonab.WebAPI.Repositories.Abstract;
using Sonab.WebAPI.Services.Abstract;

namespace Sonab.WebAPI.Services;

public class SubscriptionService : ISubsriptionService
{
    private readonly ILogger<SubscriptionService> _logger;
    private readonly IHttpContextAccessor _accessor;
    private readonly ISubscriptionRepository _repository;
    private readonly IUserRepository _userRepository;

    public SubscriptionService(
        ILogger<SubscriptionService> logger,
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

        UserSubscription subscription = new(subscriber, publisher);
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
