using Sonab.Core.Dto;
using Sonab.Core.Dto.Posts;
using Sonab.Core.Dto.Posts.Requests;
using Sonab.Core.Dto.Posts.Requests.Count;
using Sonab.Core.Dto.Posts.Requests.List;
using Sonab.Core.Dto.Posts.Responses;
using Sonab.Core.Interfaces;
using Sonab.Core.Interfaces.Repositories.ReadEntity;

namespace Sonab.Core.UseCases.Posts.List;

public class GetAllPostsUseCase : IUseCase<GetAllPostsRequest, GetPostListResponse>
{
    private readonly IPostRepository _repository;

    public GetAllPostsUseCase(IPostRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(
        LoggedInUser loggedInUser,
        GetAllPostsRequest request,
        IPresenter<GetPostListResponse> presenter)
    {
        List<PostShortInfo> result = await _repository.GetAsync(request.ListParams);
        await presenter.HandleSuccess(new GetPostListResponse(result));
    }
}
