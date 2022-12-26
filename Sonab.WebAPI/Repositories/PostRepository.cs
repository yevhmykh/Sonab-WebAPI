using Microsoft.EntityFrameworkCore;
using Sonab.WebAPI.Contexts;
using Sonab.WebAPI.Extentions;
using Sonab.WebAPI.Models;
using Sonab.WebAPI.Models.DB;
using Sonab.WebAPI.Models.Posts;
using Sonab.WebAPI.Repositories.Abstract;

namespace Sonab.WebAPI.Repositories;

public class PostRepository : BaseRepository<Post>, IPostRepository
{
    public PostRepository(AppDbContext context) : base(context)
    {
    }

    public Task<PostShortInfo[]> GetAsync(ListParams listParams) => _context.Posts
        .Include(x => x.User)
        .ApplyParams(listParams)
        .Select(x => new PostShortInfo()
        {
            Id = x.Id,
            Title = x.Title,
            Content = x.Content,
            Author = x.User.Name
        })
        .ToArrayAsync();

    public Task<PostShortInfo[]> GetUserPostsAsync(
        string externalId,
        ListParams listParams) => _context.Posts
        .Include(x => x.User)
        .Where(x => x.User.ExternalId == externalId)
        .ApplyParams(listParams)
        .Select(x => new PostShortInfo()
        {
            Id = x.Id,
            Title = x.Title,
            Content = x.Content,
            Author = x.User.Name
        })
        .ToArrayAsync();

    public Task<PostShortInfo[]> GetPublishersPostsAsync(
        string externalId,
        ListParams listParams) => _context.Posts
        .Include(x => x.User)
        .Where(x => x.User.Subscribers.Any(y => y.User.ExternalId == externalId))
        .ApplyParams(listParams)
        .Select(x => new PostShortInfo()
        {
            Id = x.Id,
            Title = x.Title,
            Content = x.Content,
            Author = x.User.Name
        })
        .ToArrayAsync();

    public Task<int> CountAsync() => _context.Posts.CountAsync();

    public Task<int> CountUserPostAsync(string externalId) => _context.Posts
        .Where(x => x.User.ExternalId == externalId)
        .CountAsync();

    public Task<int> CountPublishersPostAsync(string externalId) => _context.Posts
        .Where(x => x.User.Subscribers.Any(y => y.User.ExternalId == externalId))
        .CountAsync();

    public Task<Post> GetFullInfoAsync(int id) => _context.Posts
        .Include(x => x.User)
        .FirstOrDefaultAsync(x => x.Id == id);
}
