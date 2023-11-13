using Microsoft.EntityFrameworkCore;
using Sonab.Core.Dto.Posts;
using Sonab.Core.Entities;
using Sonab.Core.Interfaces.Repositories.ReadEntity;
using Sonab.DbRepositories.Contexts;
using Sonab.DbRepositories.Extensions;

namespace Sonab.DbRepositories.ReadEntityRepositories;

public class PostRepository : IPostRepository
{
    private readonly AppDbContext _context;
    
    public PostRepository(AppDbContext context) 
    {
        _context = context;
    }

    public Task<List<PostShortInfo>> GetAsync(PostListParams listParams) => _context.Posts
        .Include(x => x.User)
        .Include(x => x.Topics)
        .AsSplitQuery()
        .WhereIfHasValue(x => x.UserId, listParams.AuthorId)
        .WhereTopic(listParams.TopicTagId)
        .OrderByDescending(x => x.Id)
        .ApplyParams(listParams)
        .Select(x => new PostShortInfo()
        {
            Id = x.Id,
            Title = x.Title,
            Content = x.Content,
            AuthorId = x.UserId,
            Author = x.User.Name,
            Tags = x.Topics.Select(y => new TopicTag
            {
                Id = y.Id,
                Name = y.Name
            }).ToArray()
        })
        .ToListAsync();

    public Task<List<PostShortInfo>> GetUserPostsAsync(
        string externalId,
        PostListParams listParams
    ) => _context.Posts
        .Include(x => x.User)
        .Include(x => x.Topics)
        .AsSplitQuery()
        .Where(x => x.User.ExternalId == externalId)
        .WhereTopic(listParams.TopicTagId)
        .OrderByDescending(x => x.Id)
        .ApplyParams(listParams)
        .Select(x => new PostShortInfo()
        {
            Id = x.Id,
            Title = x.Title,
            Content = x.Content,
            AuthorId = x.UserId,
            Author = x.User.Name,
            Tags = x.Topics.Select(y => new TopicTag
            {
                Id = y.Id,
                Name = y.Name
            }).ToArray()
        })
        .ToListAsync();

    public Task<List<PostShortInfo>> GetPublishersPostsAsync(
        string externalId,
        PostListParams listParams
    ) => _context.Posts
        .Include(x => x.User)
        .Include(x => x.Topics)
        .AsSplitQuery()
        .Where(x => x.User.Subscribers.Any(y => y.User.ExternalId == externalId))
        .WhereTopic(listParams.TopicTagId)
        .WhereIfHasValue(x => x.UserId, listParams.AuthorId)
        .OrderByDescending(x => x.Id)
        .ApplyParams(listParams)
        .Select(x => new PostShortInfo()
        {
            Id = x.Id,
            Title = x.Title,
            Content = x.Content,
            AuthorId = x.UserId,
            Author = x.User.Name,
            Tags = x.Topics.Select(y => new TopicTag
            {
                Id = y.Id,
                Name = y.Name
            }).ToArray()
        })
        .ToListAsync();

    public Task<int> CountAsync(PostCountParams countParams) => _context.Posts
        .WhereIfHasValue(x => x.UserId, countParams.AuthorId)
        .WhereTopic(countParams.TopicTagId)
        .CountAsync();

    public Task<int> CountUserPostAsync(
        string externalId,
        PostCountParams countParams
    ) => _context.Posts
        .Where(x => x.User.ExternalId == externalId)
        .WhereTopic(countParams.TopicTagId)
        .CountAsync();

    public Task<int> CountPublishersPostAsync(
        string externalId,
        PostCountParams countParams
    ) => _context.Posts
        .Where(x => x.User.Subscribers.Any(y => y.User.ExternalId == externalId))
        .WhereTopic(countParams.TopicTagId)
        .WhereIfHasValue(x => x.UserId, countParams.AuthorId)
        .CountAsync();

    public Task<Post> GetFullInfoAsync(int id) => _context.Posts
        .Include(x => x.User)
        .Include(x => x.Topics)
        .AsSplitQuery()
        .FirstOrDefaultAsync(x => x.Id == id);
}
