using Sonab.Core.Interfaces;

namespace Sonab.Core.Dto.TopicTags.Responses;

public record GetTopicTagsResponse(List<TopicTag> TopicTags) : ResponseDto;
