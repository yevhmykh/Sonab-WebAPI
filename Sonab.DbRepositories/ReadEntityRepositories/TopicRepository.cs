using Microsoft.EntityFrameworkCore;
using Sonab.Core.Constants;
using Sonab.Core.Dto.Posts;
using Sonab.Core.Dto.TopicTags;
using Sonab.Core.Entities;
using Sonab.Core.Interfaces.Repositories.ReadEntity;
using Sonab.DbRepositories.Contexts;

namespace Sonab.DbRepositories.ReadEntityRepositories;

public class TopicRepository : ITopicRepository
{
    private readonly AppDbContext _context;
    
    public TopicRepository(AppDbContext context)
    {
        _context = context;
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
