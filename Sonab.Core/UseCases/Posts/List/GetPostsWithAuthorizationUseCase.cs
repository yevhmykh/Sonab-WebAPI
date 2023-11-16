using Sonab.Core.Dto.Posts;
using Sonab.Core.Dto.Posts.Requests;
using Sonab.Core.Dto.Posts.Requests.Count;
using Sonab.Core.Dto.Posts.Requests.List;
using Sonab.Core.Dto.Posts.Responses;
using Sonab.Core.Interfaces;
using Sonab.Core.Interfaces.Repositories.ReadEntity;

namespace Sonab.Core.UseCases.Posts.List;

public abstract class GetPostsWithAuthorizationUseCase<TRequest> : AuthorizedUseCase<TRequest, GetPostListResponse>
    where TRequest : GetPostListRequest
{
    protected readonly IPostRepository Repository;

    protected GetPostsWithAuthorizationUseCase(IPostRepository repository)
    {
        Repository = repository;
    }

    protected override async Task Handle(
        string userExternalId,
        TRequest request,
        IPresenter<GetPostListResponse> presenter)
    {
        List<PostShortInfo> result = await GetPosts(userExternalId, request.ListParams);
        await presenter.HandleSuccess(new GetPostListResponse(result));
    }

    protected abstract Task<List<PostShortInfo>> GetPosts(string userExternalId, PostListParams listParams);
}