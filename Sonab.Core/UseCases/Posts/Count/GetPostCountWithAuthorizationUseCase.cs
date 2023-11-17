using Sonab.Core.Dto.Posts.Requests;
using Sonab.Core.Dto.Posts.Requests.Count;
using Sonab.Core.Dto.Posts.Responses;
using Sonab.Core.Interfaces;
using Sonab.Core.Interfaces.Repositories.ReadEntity;

namespace Sonab.Core.UseCases.Posts.Count;

public abstract class GetPostCountWithAuthorizationUseCase : AuthorizedUseCase<GetPostCountRequest, GetPostCountResponse>
{
    protected readonly IPostRepository Repository;

    protected GetPostCountWithAuthorizationUseCase(IPostRepository repository)
    {
        Repository = repository;
    }

    protected override async Task Handle(
        string userExternalId,
        GetPostCountRequest request,
        IPresenter<GetPostCountResponse> presenter)
    {
        int count = await GetPostCount(userExternalId, request.CountParams);
        await presenter.HandleSuccess(new GetPostCountResponse(count));
    }

    protected abstract Task<int> GetPostCount(string userExternalId, PostCountParams countParams);
}
