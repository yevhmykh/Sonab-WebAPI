using Sonab.Core.Dto.Users;

namespace Sonab.Core.Interfaces.Services;

public interface IExternalAuthRepository
{
    Task<UserInfo> GetUserInfoAsync(string userId);
}
