using Sonab.Core.Interfaces;

namespace Sonab.Core.Dto.Posts.Requests.Count;

public record GetPostCountRequest(PostCountParams CountParams) : RequestDto;