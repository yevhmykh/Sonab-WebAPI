using Sonab.Core.BackgroundTasks;
using Sonab.Core.Constants;
using Sonab.Core.Dto;
using Sonab.Core.Dto.Users;
using Sonab.Core.Errors;
using Sonab.Core.Interfaces;
using Sonab.Core.Interfaces.Services;
using Sonab.Core.UseCases.Users;
using Sonab.WebAPI.Services.Background.Workers.Abstract;

namespace Sonab.WebAPI.Services.Background.Workers;

public sealed class LoadInfoWorker : ILoadInfoWorker
{
    private readonly ILogger<LoadInfoWorker> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExternalAuthRepository _externalAuthRepository;
    private readonly INotificationSender _notificationSender;

    public LoadInfoWorker(
        ILogger<LoadInfoWorker> logger,
        INotificationSender notificationSender,
        IUnitOfWork unitOfWork,
        IExternalAuthRepository externalAuthRepository)
    {
        _logger = logger;
        _notificationSender = notificationSender;
        _unitOfWork = unitOfWork;
        _externalAuthRepository = externalAuthRepository;
    }

    // TODO: object to BackgroundTask
    public async Task StartWork(object data, CancellationToken stoppingToken)
    {
        LoadUserInfoTask task = (LoadUserInfoTask)data;
        try
        {
            await LoadUserInfo(task.ExternalUserId, stoppingToken);
        }
        catch
        {
            await _notificationSender.SendErrorAsync(task.ExternalUserId, Messages.InfoNotLoaded);
            throw;
        }
    }

    private Task LoadUserInfo(string userId, CancellationToken stoppingToken)
    {
        _logger.LogInformation($"Loading info for user with ID: '{userId}'");

        LoadUserInfoUseCase useCase = new(_unitOfWork, _externalAuthRepository);
        return useCase.Handle(null, new LoadUserInfoRequest(userId), new OkPresenter(userId, _notificationSender));
    }
    
    private class OkPresenter : IPresenter<OkResponse>
    {
        private readonly string _userId;
        private readonly INotificationSender _notificationSender;

        public OkPresenter(string userId, INotificationSender notificationSender)
        {
            _userId = userId;
            _notificationSender = notificationSender;
        }
        
        public Task HandleSuccess(OkResponse response) => Task.CompletedTask;

        public Task HandleFailure(ErrorBase error) =>
            _notificationSender.SendErrorAsync(_userId, Messages.InfoNotLoaded);
    }
}
