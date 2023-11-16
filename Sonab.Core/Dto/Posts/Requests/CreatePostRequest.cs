using Sonab.Core.Interfaces;

namespace Sonab.Core.Dto.Posts.Requests;

public record CreatePostRequest(string Title, string Content, List<int> TopicTagIds, List<string> NewTopicTags) : RequestDto;
