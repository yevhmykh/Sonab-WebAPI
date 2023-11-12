using Sonab.Core.Entities;
using Sonab.WebAPI.Models.Posts;

namespace Sonab.WebAPI.Repositories.Abstract;

public interface ITopicRepository : IRepository<Topic>
{
    Task<List<Topic>> GetAsync(IEnumerable<int> ids);
    Task<List<TopicTag>> GetByAsync(string namePart);
    Task<List<TopicTag>> GetTopAsync();
}
