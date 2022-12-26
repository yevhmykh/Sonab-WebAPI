using Sonab.WebAPI.Models;
using Sonab.WebAPI.Models.DB;
using Sonab.WebAPI.Models.Posts;

namespace Sonab.WebAPI.Repositories.Abstract;

public interface IPostRepository : IRepository<Post>
{
    Task<PostShortInfo[]> GetAsync(ListParams listParams);
    Task<PostShortInfo[]> GetUserPostsAsync(string externalId, ListParams listParams);
    Task<PostShortInfo[]> GetPublishersPostsAsync( string externalId, ListParams listParams);
    Task<int> CountAsync();
    Task<int> CountUserPostAsync(string externalId);
    Task<int> CountPublishersPostAsync(string externalId);
    Task<Post> GetFullInfoAsync(int id);
}
