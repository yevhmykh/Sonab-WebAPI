using Sonab.WebAPI.Models.Auth0Communication;

namespace Sonab.WebAPI.Services.Abstract;

public interface IAuth0CommunicationService
{
    Task<UserInfo> GetUserInfoAsync(string userId);
}
