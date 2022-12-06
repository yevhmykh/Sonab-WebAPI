using Microsoft.AspNetCore.SignalR;
using Sonab.WebAPI.Extentions;
using Sonab.WebAPI.Hubs;
using Sonab.WebAPI.Models.Auth0Communication;
using Sonab.WebAPI.Models.DB;
using Sonab.WebAPI.Repositories.Abstract;
using Sonab.WebAPI.Services.Abstract;
using Sonab.WebAPI.Services.Workers.Abstract;

namespace Sonab.WebAPI.Services.Workers;

public class LoadInfoWorker : ILoadInfoWorker
{
    private readonly ILogger<LoadInfoWorker> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IAuth0CommunicationService _auth0Service;
    private readonly IHubContext<NotificationHub> _hub;

    public LoadInfoWorker(
        ILogger<LoadInfoWorker> logger,
        IUserRepository userRepository,
        IAuth0CommunicationService auth0Service,
        IHubContext<NotificationHub> hub)
    {
        _logger = logger;
        _userRepository = userRepository;
        _auth0Service = auth0Service;
        _hub = hub;
    }

    public async Task StartWork(object data, CancellationToken stoppingToken)
    {
        try
        {
            await LoadUserInfo((string)data, stoppingToken);
        }
        catch
        {
            await _hub.Clients.User((string)data)
                .SendErrorAsync("User.Error.Info");
            throw;
        }
    }

    private async Task LoadUserInfo(string userId, CancellationToken stoppingToken)
    {
        _logger.LogInformation($"Loading info for user with ID: '{userId}'");

        UserInfo info = await _auth0Service.GetUserInfoAsync(userId);
        User user = await _userRepository.GetByEmailAsync(info.Email);

        stoppingToken.ThrowIfCancellationRequested();

        if (user == null)
        {
            user = new User
            {
                Email = info.Email.ToUpper(),
                ExternalId = userId.ToUpper(),
                Name = info.UserName
            };

            await _userRepository.AddAndSaveAsync(user);
            _logger.LogInformation($"Saved with ID: {user.Id}");
        }
        else
        {
            user.ExternalId = userId.ToUpper();
            user.Name = info.UserName;

            await _userRepository.UpdateAndSaveAsync(user);
            _logger.LogInformation($"Updated by ID: {user.Id}");
        }
    }
}
