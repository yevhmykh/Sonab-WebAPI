namespace Sonab.WebAPI.Services.Background.Workers.Abstract;

public interface IWorker
{
    Task StartWork(object data, CancellationToken stoppingToken);
}
