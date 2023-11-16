namespace Sonab.Core.Dto.Posts.Requests.Count;

public record GetPublishersPostCountRequest(PostCountParams CountParams) : GetPostCountRequest(CountParams);