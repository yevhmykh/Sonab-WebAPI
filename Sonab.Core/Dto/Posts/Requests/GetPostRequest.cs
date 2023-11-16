using Sonab.Core.Interfaces;

namespace Sonab.Core.Dto.Posts.Requests;

public record GetPostRequest(int PostId) : RequestDto;