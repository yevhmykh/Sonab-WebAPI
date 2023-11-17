using Microsoft.AspNetCore.Mvc;
using Sonab.Core.Dto.TopicTags;
using Sonab.Core.Dto.TopicTags.Requests;
using Sonab.Core.Interfaces.Repositories.ReadEntity;
using Sonab.Core.Interfaces.Services;
using Sonab.Core.UseCases.TopicTags;
using Sonab.WebAPI.Presenters.TopicTags;

namespace Sonab.WebAPI.Controllers;

[ApiController]
[Route("topicTag")]
public class TopicController : BaseController
{
    private readonly ILogger<TopicController> _logger;
    private readonly IDataMemoryService _dataMemoryService;
    private readonly ITopicRepository _repository;

    public TopicController(
        ILogger<TopicController> logger,
        IDataMemoryService dataMemoryService,
        ITopicRepository repository)
    {
        _logger = logger;
        _dataMemoryService = dataMemoryService;
        _repository = repository;
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(TopicTag[]), StatusCodes.Status200OK)]
    public Task<IActionResult> GetListAsync([FromQuery] string namePart) =>
        ExecuteUseCase(
            new GetTopicTagsRequest(namePart),
            string.IsNullOrEmpty(namePart)
                ? new GetTopTopicTagsUseCase(_dataMemoryService, _repository)
                : new GetTopicTagsUseCase(_repository),
            new TopicTagsPresenter());
}
