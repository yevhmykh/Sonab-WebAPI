using Sonab.Core.Interfaces;

namespace Sonab.Core.Dto.TopicTags.Requests;

public record GetTopicTagsRequest(string NamePart) : RequestDto;
