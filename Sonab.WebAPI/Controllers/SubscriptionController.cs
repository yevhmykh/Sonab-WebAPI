using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sonab.Core.Dto.Subscriptions;
using Sonab.WebAPI.Models;
using Sonab.WebAPI.Services.Abstract;

namespace Sonab.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("subscriptions")]
public class SubscriptionController : ControllerBase
{
    private readonly ILogger<SubscriptionController> _logger;
    private readonly ISubsriptionService _service;

    public SubscriptionController(
        ILogger<SubscriptionController> logger,
        ISubsriptionService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet("list")]
    [ProducesResponseType(typeof(SubscriptionFullInfo[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAsync()
    {
        ServiceResponse result = await _service.GetAsync();

        return Ok(result.Data);
    }

    [HttpPost("subscribe/{publisherId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> SubscribeAsync([FromRoute] int publisherId)
    {
        ServiceResponse result = await _service.SubscribeAsync(publisherId);

        return result.IsSuccess() ?
            Ok() :
            StatusCode(result.StatusCode, result.Messages);
    }

    [HttpDelete("unsubscribe/{publisherId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UnsubscribeAsync([FromRoute] int publisherId)
    {
        ServiceResponse result = await _service.UnsubscribeAsync(publisherId);

        return result.IsSuccess() ?
            Ok() :
            StatusCode(result.StatusCode, result.Messages);
    }
}
