namespace Sonab.Core.Dto.Posts.Requests.Count;

public record GetUserPostCountRequest(PostCountParams CountParams) : GetPostCountRequest(CountParams);