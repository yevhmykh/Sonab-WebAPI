using Sonab.Core.Interfaces;

namespace Sonab.Core.Dto.Posts.Responses;

public record GetPostResponse(PostFullInfo Post) : ResponseDto;