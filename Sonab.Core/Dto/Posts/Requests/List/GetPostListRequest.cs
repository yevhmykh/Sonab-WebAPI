using Sonab.Core.Interfaces;

namespace Sonab.Core.Dto.Posts.Requests.List;

public abstract record GetPostListRequest(PostListParams ListParams) : RequestDto;
