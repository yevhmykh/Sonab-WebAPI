using Microsoft.Extensions.Logging;
using Sonab.Auth0Client.Models;
using Sonab.Core.Dto.Users;
using Sonab.Core.Interfaces.Services;

namespace Sonab.Auth0Client;

public class Auth0AuthRepository : IExternalAuthRepository
{
    private readonly ILogger<Auth0AuthRepository> _logger;
    private readonly Auth0Options _options;
    private readonly IRequestClient _requestClient;
    private DateTime ExpirationTime { get; set; }
    private TokenData TokenData { get; set; }

    public Auth0AuthRepository(
        ILogger<Auth0AuthRepository> logger,
        Auth0Options options,
        IRequestClient requestClient)
    {
        _logger = logger;
        _options = options;
        _requestClient = requestClient;
    }

    public async Task<UserInfo> GetUserInfoAsync(string userId)
    {
        if (ExpirationTime <= DateTime.Now)
        {
            _logger.LogDebug("Loading new access token.");
            Dictionary<string, string> data = new()
            {
                { "Content-Type", "application/x-www-form-urlencoded" },
                { "grant_type", "client_credentials" },
                { "client_id", _options.ClientId },
                { "client_secret", _options.ClientSecret },
                { "audience", $"{_options.Domain}/api/v2/" },
            };
            TokenData = await _requestClient.PostRequestAsync<TokenData>(
                $"{_options.Domain}/oauth/token",
                data);

            ExpirationTime = DateTime.Now.AddSeconds(TokenData.ExpiresIn);
            if (!TokenData.Scope.Contains("read:users"))
            {
                throw new InvalidOperationException("Required scope missing");
            }
            _logger.LogDebug($"Token has updated. New expiration date: {ExpirationTime}");
        }

        UserData userData = await _requestClient.GetRequestAsync<UserData>(
            $"{_options.Domain}/api/v2/users/{userId.ToLower()}",
            TokenData.AccessToken);
        _logger.LogDebug("User information successfully loaded.");
        return new UserInfo(userData.Email, userData.UserName);
    }
}
