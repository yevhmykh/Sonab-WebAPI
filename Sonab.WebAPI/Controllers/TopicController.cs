using Microsoft.AspNetCore.Mvc;
using Sonab.Core.Dto.Posts;
using Sonab.Core.Dto.TopicTags;
using Sonab.WebAPI.Models;
using Sonab.WebAPI.Services.Abstract;

namespace Sonab.WebAPI.Controllers;

[ApiController]
[Route("topicTag")]
public class TopicController : ControllerBase
{
    private readonly ILogger<TopicController> _logger;
    private readonly ITopicService _service;

    public TopicController(
        ILogger<TopicController> logger,
        ITopicService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(TopicTag[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetListAsync([FromQuery] string namePart)
    {
        ServiceResponse result = await _service.GetListAsync(namePart);

        return Ok(result.Data);
    }
}
