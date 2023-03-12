using Microsoft.EntityFrameworkCore;
using Sonab.WebAPI.Contexts;
using Sonab.WebAPI.Models.DB;
using Sonab.WebAPI.Models.Posts;
using Sonab.WebAPI.Repositories.Abstract;

namespace Sonab.WebAPI.Repositories;

public class TopicRepository : BaseRepository<Topic>, ITopicRepository
{
    public TopicRepository(AppDbContext context) : base(context)
    {
    }

    public Task<List<Topic>> GetAsync(IEnumerable<int> ids) => _context.Topics
        .Where(x => ids.Contains(x.Id))
        .ToListAsync();

    public Task<List<TopicTag>> GetByAsync(string namePart) => _context.Topics
        .Where(x => string.IsNullOrEmpty(namePart)
            || x.NormalizedName.Contains(namePart.ToUpper()))
        .Select(x => new TopicTag
        {
            Id = x.Id,
            Name = x.Name
        })
        .Take(Limits.TopicCount)
        .ToListAsync();

    public Task<List<TopicTag>> GetTopAsync() => _context.Topics
        .OrderByDescending(x => x.Posts.Count)
        .Select(x => new TopicTag
        {
            Id = x.Id,
            Name = x.Name
        })
        .Take(Limits.TopicCount)
        .ToListAsync();
}
