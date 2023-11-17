using Sonab.Core.Dto.TopicTags;

namespace Sonab.Core.Interfaces.Services;

public interface IDataMemoryService
{
    List<TopicTag> GetTopTopics();
    void SaveTopTopics(List<TopicTag> topicTags);
}
