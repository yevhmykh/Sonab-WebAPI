namespace Sonab.WebAPI.Services.Workers.Abstract;

public interface IWorker
{
    Task StartWork(object data, CancellationToken stoppingToken);
}
