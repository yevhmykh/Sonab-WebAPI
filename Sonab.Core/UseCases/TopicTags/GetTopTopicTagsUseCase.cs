using Sonab.Core.Dto;
using Sonab.Core.Dto.TopicTags;
using Sonab.Core.Dto.TopicTags.Requests;
using Sonab.Core.Dto.TopicTags.Responses;
using Sonab.Core.Interfaces;
using Sonab.Core.Interfaces.Repositories.ReadEntity;

namespace Sonab.Core.UseCases.TopicTags;

public class GetTopTopicTagsUseCase : IUseCase<GetTopTopicTagsRequest, GetTopicTagsResponse>
{
    private readonly ITopicRepository _repository;

    public GetTopTopicTagsUseCase(ITopicRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(
        LoggedInUser loggedInUser,
        GetTopTopicTagsRequest request,
        IPresenter<GetTopicTagsResponse> presenter)
    {
        List<TopicTag> result = await _repository.GetTopAsync();
        await presenter.HandleSuccess(new GetTopicTagsResponse(result));
    }
}