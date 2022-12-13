using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sonab.WebAPI.Models;
using Sonab.WebAPI.Models.Posts;
using Sonab.WebAPI.Services.Abstract;

namespace Sonab.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("posts")]
public class PostController : ControllerBase
{
    private readonly ILogger<PostController> _logger;
    private readonly IPostService _service;

    public PostController(
        ILogger<PostController> logger,
        IPostService service)
    {
        _logger = logger;
        _service = service;
    }

    [AllowAnonymous]
    [HttpGet("all/list")]
    [ProducesResponseType(typeof(PostShortInfo[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAsync([FromQuery] ListParams listParams)
    {
        ServiceResponse result = await _service.GetListAsync(listParams);

        return Ok(result.Data);
    }

    [HttpGet("user/list")]
    [ProducesResponseType(typeof(PostShortInfo[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserPostsAsync([FromQuery] ListParams listParams)
    {
        ServiceResponse result = await _service.GetUserPostsAsync(listParams);

        return Ok(result.Data);
    }

    [AllowAnonymous]
    [HttpGet("all/count")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<IActionResult> CountAsync()
    {
        ServiceResponse result = await _service.CountAsync();

        return Ok(result.Data);
    }

    [HttpGet("user/count")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<IActionResult> CountUserPostAsync()
    {
        ServiceResponse result = await _service.CountUserPostAsync();

        return Ok(result.Data);
    }

    [AllowAnonymous]
    [HttpGet("get/{id:int}")]
    [ProducesResponseType(typeof(PostFullInfo), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        ServiceResponse result = await _service.GetByIdAsync(id);

        return result.IsSuccess() ?
            Ok(result.Data) :
            StatusCode(result.StatusCode, result.Messages);
    }

    [HttpPost("create")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateAsync([FromBody] EditRequest request)
    {
        ServiceResponse result = await _service.CreateAsync(request);

        return result.IsSuccess() ?
            Ok(result.Data) :
            StatusCode(result.StatusCode, result.Messages);
    }

    [HttpPut("edit/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EditAsync([FromRoute] int id, [FromBody] EditRequest request)
    {
        ServiceResponse result = await _service.UpdateAsync(id, request);

        return result.IsSuccess() ?
            Ok() :
            StatusCode(result.StatusCode, result.Messages);
    }

    [HttpDelete("remove/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveAsync([FromRoute] int id)
    {
        ServiceResponse result = await _service.RemoveAsync(id);

        return result.IsSuccess() ?
            Ok() :
            StatusCode(result.StatusCode, result.Messages);
    }
}
