using Sonab.Core.Interfaces;

namespace Sonab.Core.Dto.Posts.Requests;

public record RemovePostRequest(int PostId) : RequestDto;