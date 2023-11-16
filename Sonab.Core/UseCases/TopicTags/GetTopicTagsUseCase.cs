using Sonab.Core.Dto;
using Sonab.Core.Dto.TopicTags;
using Sonab.Core.Dto.TopicTags.Requests;
using Sonab.Core.Dto.TopicTags.Responses;
using Sonab.Core.Interfaces;
using Sonab.Core.Interfaces.Repositories.ReadEntity;

namespace Sonab.Core.UseCases.TopicTags;

public class GetTopicTagsUseCase : IUseCase<GetTopicTagsRequest, GetTopicTagsResponse>
{
    private readonly ITopicRepository _repository;

    public GetTopicTagsUseCase(ITopicRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(
        LoggedInUser loggedInUser,
        GetTopicTagsRequest request,
        IPresenter<GetTopicTagsResponse> presenter)
    {
        List<TopicTag> result = await _repository.GetByAsync(request.NamePart);
        await presenter.HandleSuccess(new GetTopicTagsResponse(result));
    }
}
