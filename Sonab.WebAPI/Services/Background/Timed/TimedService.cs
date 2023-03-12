using Sonab.WebAPI.Services.Background.Workers.Abstract;

namespace Sonab.WebAPI.Services.Background.Timed;

public abstract class TimedService<TWorker> : BackgroundService where TWorker : IWorker
{
    private readonly TimeSpan _delay;
    protected readonly ILogger<TimedService<TWorker>> _logger;

    public TimedService(
        TimeSpan delay,
        IServiceProvider services,
        ILogger<TimedService<TWorker>> logger)
    {
        _delay = delay;
        Services = services;
        _logger = logger;
    }

    private IServiceProvider Services { get; }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"Starting {GetType().Name}.");
        await DoWork(stoppingToken);
    }

    private async Task DoWork(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = Services.CreateScope())
            {
                TWorker worker = scope.ServiceProvider.GetRequiredService<TWorker>();

                await worker.StartWork(null, stoppingToken);
            }

            await Task.Delay(_delay, stoppingToken);
        }
    }
}
