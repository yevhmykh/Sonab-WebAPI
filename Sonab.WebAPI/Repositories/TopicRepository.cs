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

    public Task<Topic[]> GetAsync(IEnumerable<int> ids) => _context.Topics
        .Where(x => ids.Contains(x.Id))
        .ToArrayAsync();

    public Task<TopicTag[]> GetByPartAsync(string namePart) => _context.Topics
        .Where(x => string.IsNullOrEmpty(namePart)
            || x.NormalizedName.Contains(namePart.ToUpper()))
        .Select(x => new TopicTag
        {
            Id = x.Id,
            Name = x.Name
        })
        .ToArrayAsync();
}
