using Sonab.Core.Interfaces;

namespace Sonab.Core.Dto.Users;

public record LoadUserInfoRequest(string ExternalUserId) : RequestDto;
