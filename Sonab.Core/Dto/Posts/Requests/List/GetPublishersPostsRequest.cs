namespace Sonab.Core.Dto.Posts.Requests.List;

public record GetPublishersPostsRequest(PostListParams ListParams) : GetPostListRequest(ListParams);