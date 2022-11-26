using Sonab.WebAPI.Enums;
using Sonab.WebAPI.Models;
using Sonab.WebAPI.Services.Workers.Abstract;
using Sonab.WebAPI.Utils.Abstact;

namespace Sonab.WebAPI.Services;

public class BackgroundWorker : BackgroundService
{
    private readonly ILogger<BackgroundWorker> _logger;
    private readonly IBackgroundQueue _queue;
    private readonly IServiceScopeFactory _scopeFactory;

    public BackgroundWorker(
        ILogger<BackgroundWorker> logger,
        IBackgroundQueue queue,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _queue = queue;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting background worker is starting....");

        await BackgroundProcessing(stoppingToken);
    }

    private async Task BackgroundProcessing(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                JobInfo job = await _queue.Dequeue();

                _logger.LogInformation($"Job {job} found! Starting to process ...");

                using (IServiceScope scope = _scopeFactory.CreateScope())
                {
                    IWorker worker = GetWorker(scope, job.Type);

                    await worker.StartWork(job.Data, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred when running worker.");
            }
        }
    }

    private IWorker GetWorker(IServiceScope scope, JobType job) => job switch
    {
        JobType.LoadInfo => scope.ServiceProvider.GetRequiredService<ILoadInfoWorker>(),
        _ => throw new ArgumentException("Unknown job type")
    };
}
