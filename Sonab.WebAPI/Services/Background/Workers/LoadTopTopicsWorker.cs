using Microsoft.Extensions.Caching.Memory;
using Sonab.Core.Constants;
using Sonab.Core.Dto.Posts;
using Sonab.Core.Interfaces.Repositories;
using Sonab.Core.Interfaces.Repositories.ReadEntity;
using Sonab.WebAPI.Repositories.Abstract;
using Sonab.WebAPI.Services.Background.Workers.Abstract;

namespace Sonab.WebAPI.Services.Background.Workers;

public sealed class LoadTopTopicsWorker : ILoadTopTopicsWorker
{
    private readonly ILogger<LoadTopTopicsWorker> _logger;
    private readonly IMemoryCache _memoryCache;
    private readonly ITopicRepository _topicRepository;

    public LoadTopTopicsWorker(
        ILogger<LoadTopTopicsWorker> logger,
        IMemoryCache memoryCache,
        ITopicRepository topicRepository)
    {
        _logger = logger;
        _memoryCache = memoryCache;
        _topicRepository = topicRepository;
    }

    public async Task StartWork(object data, CancellationToken stoppingToken)
    {
        _logger.LogDebug("Loading most popular topic tags");
        List<TopicTag> tags = await _topicRepository.GetTopAsync();

        _memoryCache.Set(
            CacheValues.TopTopics,
            tags,
            TimeSpan.FromMinutes(TimedPeriods.Topic + TimedPeriods.Spare));
        _logger.LogDebug($"Cached {tags.Count} tags.");
    }
}
