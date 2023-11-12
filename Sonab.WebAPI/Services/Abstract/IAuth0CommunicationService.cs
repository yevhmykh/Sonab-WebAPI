using Sonab.WebAPI.Models.Auth0Communication;

namespace Sonab.WebAPI.Services.Abstract;

// TODO: replace by external repository
public interface IAuth0CommunicationService
{
    Task<UserInfo> GetUserInfoAsync(string userId);
}
