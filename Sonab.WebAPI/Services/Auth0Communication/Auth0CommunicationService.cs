using Sonab.WebAPI.Models.Auth0Communication;
using Sonab.WebAPI.Services.Abstract;
using Sonab.WebAPI.Utils.Abstact;

namespace Sonab.WebAPI.Services.Auth0Communication;

public class Auth0CommunicationService : IAuth0CommunicationService
{
    private readonly ILogger<Auth0CommunicationService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IRequestClient _requestClient;
    private DateTime ExpirationTime { get; set; }
    private TokenData TokenData { get; set; }

    public Auth0CommunicationService(
        ILogger<Auth0CommunicationService> logger,
        IConfiguration configuration,
        IRequestClient requestClient)
    {
        _logger = logger;
        _configuration = configuration;
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
                { "client_id", _configuration["Auth0:ClientId"] },
                { "client_secret", _configuration["Auth0:ClientSecret"] },
                { "audience", $"{_configuration["Auth0:Domain"]}/api/v2/" },
            };
            TokenData = await _requestClient.PostRequestAsync<TokenData>(
                $"{_configuration["Auth0:Domain"]}/oauth/token",
                data);

            ExpirationTime = DateTime.Now.AddSeconds(TokenData.ExpiresIn);
            if (!TokenData.Scope.Contains("read:users"))
            {
                throw new InvalidOperationException("Required scope missing");
            }
            _logger.LogDebug($"Token has updated. New expiration date: {ExpirationTime}");
        }

        UserInfo userInfo = await _requestClient.GetRequestAsync<UserInfo>(
            $"{_configuration["Auth0:Domain"]}/api/v2/users/{userId.ToLower()}",
            TokenData.AccessToken);
        _logger.LogDebug("User information successfully loaded.");
        return userInfo;
    }
}
