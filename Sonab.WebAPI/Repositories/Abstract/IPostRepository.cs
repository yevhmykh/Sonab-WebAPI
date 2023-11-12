using Sonab.Core.Entities;
using Sonab.WebAPI.Models.Posts;

namespace Sonab.WebAPI.Repositories.Abstract;

public interface IPostRepository : IRepository<Post>
{
    Task<List<PostShortInfo>> GetAsync(PostListParams listParams);
    Task<List<PostShortInfo>> GetUserPostsAsync(string externalId, PostListParams listParams);
    Task<List<PostShortInfo>> GetPublishersPostsAsync( string externalId, PostListParams listParams);
    Task<int> CountAsync(PostCountParams countParams);
    Task<int> CountUserPostAsync(string externalId, PostCountParams countParams);
    Task<int> CountPublishersPostAsync(string externalId, PostCountParams countParams);
    Task<Post> GetFullInfoAsync(int id);
}
