using Sonab.Core.Dto.TopicTags;
using Sonab.Core.Dto.TopicTags.Responses;

namespace Sonab.WebAPI.Presenters.TopicTags;

public class TopicTagsPresenter : ListApiPresenter<GetTopicTagsResponse, TopicTag>
{
    protected override List<TopicTag> GetItems(GetTopicTagsResponse response) => response.TopicTags;
}
