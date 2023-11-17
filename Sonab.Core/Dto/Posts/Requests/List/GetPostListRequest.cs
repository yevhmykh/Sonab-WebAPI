using Sonab.Core.Interfaces;

namespace Sonab.Core.Dto.Posts.Requests.List;

public record GetPostListRequest(PostListParams ListParams) : RequestDto;
