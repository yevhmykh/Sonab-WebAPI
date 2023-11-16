using Sonab.Core.Interfaces;

namespace Sonab.Core.Dto.Posts.Responses;

public record GetPostCountResponse(int Count) : ResponseDto;