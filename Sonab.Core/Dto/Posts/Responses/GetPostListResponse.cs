using Sonab.Core.Interfaces;

namespace Sonab.Core.Dto.Posts.Responses;

public record GetPostListResponse(List<PostShortInfo> Posts) : ResponseDto;
