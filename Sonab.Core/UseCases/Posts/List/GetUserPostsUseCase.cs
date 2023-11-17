using Sonab.Core.Dto.Posts;
using Sonab.Core.Dto.Posts.Requests;
using Sonab.Core.Interfaces.Repositories.ReadEntity;

namespace Sonab.Core.UseCases.Posts.List;

public class GetUserPostsUseCase : GetPostsWithAuthorizationUseCase
{
    public GetUserPostsUseCase(IPostRepository repository) : base(repository)
    {
    }

    protected override Task<List<PostShortInfo>> GetPosts(string userExternalId, PostListParams listParams) =>
        Repository.GetUserPostsAsync(userExternalId, listParams);
}
