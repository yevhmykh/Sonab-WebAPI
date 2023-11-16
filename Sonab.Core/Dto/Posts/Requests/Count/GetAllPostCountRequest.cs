namespace Sonab.Core.Dto.Posts.Requests.Count;

public record GetAllPostCountRequest(PostCountParams CountParams) : GetPostCountRequest(CountParams);