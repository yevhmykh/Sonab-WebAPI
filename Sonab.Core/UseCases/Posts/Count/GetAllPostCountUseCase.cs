using Sonab.Core.Dto;
using Sonab.Core.Dto.Posts.Requests.Count;
using Sonab.Core.Dto.Posts.Responses;
using Sonab.Core.Interfaces;
using Sonab.Core.Interfaces.Repositories.ReadEntity;

namespace Sonab.Core.UseCases.Posts.Count;

public class GetAllPostCountUseCase : IUseCase<GetPostCountRequest, GetPostCountResponse>
{
    private readonly IPostRepository _repository;

    public GetAllPostCountUseCase(IPostRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(
        LoggedInUser loggedInUser,
        GetPostCountRequest request,
        IPresenter<GetPostCountResponse> presenter)
    {
        int count = await _repository.CountAsync(request.CountParams);
        await presenter.HandleSuccess(new GetPostCountResponse(count));
    }
}
