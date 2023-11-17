using Sonab.Core.Dto;
using Sonab.Core.Dto.TopicTags;
using Sonab.Core.Dto.TopicTags.Requests;
using Sonab.Core.Dto.TopicTags.Responses;
using Sonab.Core.Interfaces;
using Sonab.Core.Interfaces.Repositories.ReadEntity;
using Sonab.Core.Interfaces.Services;

namespace Sonab.Core.UseCases.TopicTags;

public class GetTopTopicTagsUseCase : IUseCase<GetTopicTagsRequest, GetTopicTagsResponse>
{
    private readonly IDataMemoryService _dataMemoryService;
    private readonly ITopicRepository _repository;

    public GetTopTopicTagsUseCase(IDataMemoryService dataMemoryService, ITopicRepository repository)
    {
        _dataMemoryService = dataMemoryService;
        _repository = repository;
    }

    public async Task Handle(
        LoggedInUser loggedInUser,
        GetTopicTagsRequest request,
        IPresenter<GetTopicTagsResponse> presenter)
    {
        List<TopicTag> result = _dataMemoryService.GetTopTopics();
        if (result == null)
        {
            result = await _repository.GetTopAsync();
            _dataMemoryService.SaveTopTopics(result);
        }
        await presenter.HandleSuccess(new GetTopicTagsResponse(result));
    }
}
