using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sonab.Core.Dto.Users;
using Sonab.Core.Dto.Users.Subscriptions;
using Sonab.Core.Dto.Users.Subscriptions.Requests;
using Sonab.Core.Interfaces;
using Sonab.Core.Interfaces.Repositories.ReadEntity;
using Sonab.Core.Interfaces.Services;
using Sonab.Core.UseCases.Users;
using Sonab.Core.UseCases.Users.Subscriptions;
using Sonab.WebAPI.Presenters;
using Sonab.WebAPI.Presenters.Users;

namespace Sonab.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UserController : BaseController
{
    private readonly ILogger<UserController> _logger;
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISubscriptionRepository _subscriptionRepository;

    public UserController(
        ILogger<UserController> logger,
        IBackgroundTaskQueue taskQueue,
        IUnitOfWork unitOfWork,
        ISubscriptionRepository subscriptionRepository)
    {
        _logger = logger;
        _taskQueue = taskQueue;
        _unitOfWork = unitOfWork;
        _subscriptionRepository = subscriptionRepository;
    }

    [HttpPost("load-info")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<IActionResult> RequestLoading()
    {
        _logger.LogDebug("Loading user info request.");

        return ExecuteUseCase(
            new StartLoadUserInfoRequest(),
            new StartLoadUserInfoUseCase(_taskQueue),
            new OkResponsePresenter()
        );
    }

    [HttpGet("subscriptions")]
    [ProducesResponseType(typeof(SubscriptionFullInfo[]), StatusCodes.Status200OK)]
    public Task<IActionResult> GetSubscriptionsAsync()
    {
        return ExecuteUseCase(
            new GetUserSubscriptionsRequest(),
            new GetUserSubscriptionsUseCase(_subscriptionRepository),
            new SubscriptionsPresenter()
        );
    }

    [HttpPost("subscribe/{publisherId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public Task<IActionResult> SubscribeAsync([FromRoute] int publisherId)
    {
        return ExecuteUseCase(
            new SubscribeRequest(publisherId),
            new SubscribeUseCase(_unitOfWork),
            new SubscribePresenter()
        );
    }

    [HttpDelete("unsubscribe/{publisherId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public Task<IActionResult> UnsubscribeAsync([FromRoute] int publisherId)
    {
        return ExecuteUseCase(
            new UnsubscribeRequest(publisherId),
            new UnsubscribeUseCase(_unitOfWork),
            new UnsubscribePresenter()
        );
    }
}
