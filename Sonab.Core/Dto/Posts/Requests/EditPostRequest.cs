using Sonab.Core.Interfaces;

namespace Sonab.Core.Dto.Posts.Requests;

public record EditPostRequest(int PostId, string Title, string Content, List<int> TopicTagIds, List<string> NewTopicTags) : RequestDto;
