using Microsoft.Extensions.Caching.Memory;
using Sonab.Core.Constants;
using Sonab.Core.Dto.TopicTags;
using Sonab.Core.Interfaces.Services;

namespace Sonab.WebAPI.Services;

public class AspNetDataMemoryService : IDataMemoryService
{
    /// <summary>
    /// Top topics cache value
    /// </summary>
    private const string TopTopics = "_top_topics";
    
    private readonly IMemoryCache _memoryCache;

    public AspNetDataMemoryService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public List<TopicTag> GetTopTopics() => _memoryCache.Get<List<TopicTag>>(TopTopics);
    public void SaveTopTopics(List<TopicTag> topicTags) =>
        _memoryCache.Set(
            TopTopics,
            topicTags,
            TimeSpan.FromMinutes(TimedPeriods.Topic + TimedPeriods.Spare));
}
