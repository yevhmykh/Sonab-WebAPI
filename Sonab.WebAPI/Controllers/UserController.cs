using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sonab.WebAPI.Extentions;
using Sonab.WebAPI.Services.Abstract;

namespace Sonab.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _service;

    public UserController(
        ILogger<UserController> logger,
        IUserService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpPost("load-info")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult RequestLoading()
    {
        _logger.LogDebug("Loading user info request.");
        _service.RequestLoading(HttpContext.User.GetUserId());

        return Ok();
    }
}
