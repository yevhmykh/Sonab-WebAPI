using Sonab.Core.Dto.Posts;
using Sonab.Core.Dto.TopicTags;
using Sonab.Core.Entities;

namespace Sonab.Core.Interfaces.Repositories.ReadEntity;

public interface ITopicRepository
{
    Task<List<Topic>> GetAsync(IEnumerable<int> ids);
    Task<List<TopicTag>> GetByAsync(string namePart);
    Task<List<TopicTag>> GetTopAsync();
}
