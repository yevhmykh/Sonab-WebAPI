using Sonab.Core.Dto.Posts;
using Sonab.Core.Dto.Posts.Requests;
using Sonab.Core.Dto.Posts.Requests.Count;
using Sonab.Core.Dto.Posts.Requests.List;
using Sonab.Core.Interfaces.Repositories.ReadEntity;

namespace Sonab.Core.UseCases.Posts.List;

public class GetPublishersPostsUseCase : GetPostsWithAuthorizationUseCase<GetPublishersPostsRequest>
{
    public GetPublishersPostsUseCase(IPostRepository repository) : base(repository)
    {
    }

    protected override Task<List<PostShortInfo>> GetPosts(string userExternalId, PostListParams listParams) =>
        Repository.GetUserPostsAsync(userExternalId, listParams);
}