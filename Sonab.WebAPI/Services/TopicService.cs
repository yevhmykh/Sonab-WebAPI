using Sonab.WebAPI.Models;
using Sonab.WebAPI.Models.Posts;
using Sonab.WebAPI.Repositories.Abstract;
using Sonab.WebAPI.Services.Abstract;

namespace Sonab.WebAPI.Services;

public class TopicService : ITopicService
{
    private readonly ILogger<TopicService> _logger;
    private readonly ITopicRepository _repository;

    public TopicService(
        ILogger<TopicService> logger,
        ITopicRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<ServiceResponse> GetListAsync(string namePart)
    {
        TopicTag[] tags = await _repository.GetByPartAsync(namePart);
        return ServiceResponse.CreateOk(tags);
    }
}
