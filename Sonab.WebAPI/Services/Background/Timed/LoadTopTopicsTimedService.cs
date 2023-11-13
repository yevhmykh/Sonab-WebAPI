using Sonab.Core.Constants;
using Sonab.WebAPI.Services.Background.Workers;
using Sonab.WebAPI.Services.Background.Workers.Abstract;

namespace Sonab.WebAPI.Services.Background.Timed;

public sealed class LoadTopTopicsTimedService : TimedService<ILoadTopTopicsWorker>
{
    public LoadTopTopicsTimedService(
        IServiceProvider services,
        ILogger<LoadTopTopicsTimedService> logger
    ) : base(TimeSpan.FromMinutes(TimedPeriods.Topic), services, logger)
    {
    }
}
