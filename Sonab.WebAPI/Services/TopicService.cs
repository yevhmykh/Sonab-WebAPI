using Microsoft.Extensions.Caching.Memory;
using Sonab.Core.Constants;
using Sonab.Core.Dto.Posts;
using Sonab.Core.Interfaces.Repositories;
using Sonab.Core.Interfaces.Repositories.ReadEntity;
using Sonab.WebAPI.Models;
using Sonab.WebAPI.Repositories.Abstract;
using Sonab.WebAPI.Services.Abstract;

namespace Sonab.WebAPI.Services;

public class TopicService : ITopicService
{
    private readonly ILogger<TopicService> _logger;
    private readonly IMemoryCache _memoryCache;
    private readonly ITopicRepository _repository;

    public TopicService(
        ILogger<TopicService> logger,
        IMemoryCache memoryCache,
        ITopicRepository repository)
    {
        _logger = logger;
        _memoryCache = memoryCache;
        _repository = repository;
    }

    public async Task<ServiceResponse> GetListAsync(string namePart)
    {
        List<TopicTag> tags = string.IsNullOrEmpty(namePart)
            ? await GetTopAsync()
            : await _repository.GetByAsync(namePart);
        return ServiceResponse.CreateOk(tags);
    }

    private async Task<List<TopicTag>> GetTopAsync()
    {
        return _memoryCache.Get<List<TopicTag>>(CacheValues.TopTopics)
            ?? await _repository.GetTopAsync();
    }
}
