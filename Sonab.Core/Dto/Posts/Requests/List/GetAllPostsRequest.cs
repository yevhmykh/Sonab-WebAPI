namespace Sonab.Core.Dto.Posts.Requests.List;

public record GetAllPostsRequest(PostListParams ListParams) : GetPostListRequest(ListParams);