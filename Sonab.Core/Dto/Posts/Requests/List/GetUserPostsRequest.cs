namespace Sonab.Core.Dto.Posts.Requests.List;

public record GetUserPostsRequest(PostListParams ListParams) : GetPostListRequest(ListParams);