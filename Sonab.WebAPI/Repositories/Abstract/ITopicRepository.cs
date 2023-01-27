using Sonab.WebAPI.Models.DB;
using Sonab.WebAPI.Models.Posts;

namespace Sonab.WebAPI.Repositories.Abstract;

public interface ITopicRepository : IRepository<Topic>
{
    Task<Topic[]> GetAsync(IEnumerable<int> ids);
    Task<TopicTag[]> GetByPartAsync(string namePart);
}
