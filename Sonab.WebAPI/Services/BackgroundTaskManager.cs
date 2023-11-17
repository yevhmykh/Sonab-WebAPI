using Sonab.Core.BackgroundTasks;
using Sonab.Core.Interfaces.Services;
using Sonab.WebAPI.Services.Background.Workers.Abstract;

namespace Sonab.WebAPI.Services;

public sealed class BackgroundTaskManager : BackgroundService
{
    private readonly ILogger<BackgroundTaskManager> _logger;
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly IServiceScopeFactory _scopeFactory;

    public BackgroundTaskManager(
        ILogger<BackgroundTaskManager> logger,
        IBackgroundTaskQueue queue,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _taskQueue = queue;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting background task manager....");

        await BackgroundProcessing(stoppingToken);
    }

    private async Task BackgroundProcessing(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                BackgroundTask task = await _taskQueue.Dequeue();

                _logger.LogInformation($"Task {task.GetType().Name} found! Starting to process ...");

                using IServiceScope scope = _scopeFactory.CreateScope();
                IWorker worker = GetWorker(scope, task);

                await worker.StartWork(task, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred when running worker.");
            }
        }
    }

    private static IWorker GetWorker(IServiceScope scope, BackgroundTask task) => task switch
    {
        LoadUserInfoTask => scope.ServiceProvider.GetRequiredService<ILoadInfoWorker>(),
        _ => throw new ArgumentException("Unknown task type")
    };
}
