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

    public LoadInfoWorker(
        ILogger<LoadInfoWorker> logger,
        IUserRepository userRepository,
        IAuth0CommunicationService auth0Service)
    {
        _logger = logger;
        _userRepository = userRepository;
        _auth0Service = auth0Service;
    }

    public async Task StartWork(object data, CancellationToken stoppingToken)
    {
        string userId = (string)data;
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
