using Sonab.Core.Dto.Posts;
using Sonab.Core.Dto.Posts.Requests;
using Sonab.Core.Entities;

namespace Sonab.Core.Interfaces.Repositories.ReadEntity;

public interface IPostRepository
{
    Task<List<PostShortInfo>> GetAsync(PostListParams listParams);
    Task<List<PostShortInfo>> GetUserPostsAsync(string externalId, PostListParams listParams);
    Task<List<PostShortInfo>> GetPublishersPostsAsync( string externalId, PostListParams listParams);
    Task<int> CountAsync(PostCountParams countParams);
    Task<int> CountUserPostAsync(string externalId, PostCountParams countParams);
    Task<int> CountPublishersPostAsync(string externalId, PostCountParams countParams);
    Task<Post> GetFullInfoAsync(int id);
}
