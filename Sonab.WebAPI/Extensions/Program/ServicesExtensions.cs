using Sonab.Auth0Client;
using Sonab.Core.Interfaces;
using Sonab.Core.Interfaces.Repositories.ReadEntity;
using Sonab.Core.Interfaces.Services;
using Sonab.DbRepositories;
using Sonab.DbRepositories.ReadEntityRepositories;
using Sonab.WebAPI.Services;
using Sonab.WebAPI.Services.Background.Timed;
using Sonab.WebAPI.Services.Background.Workers;
using Sonab.WebAPI.Services.Background.Workers.Abstract;
using Sonab.WebAPI.Utils.Queue;
using Sonab.WebAPI.Utils.RequestClients;

namespace Sonab.WebAPI.Extensions.Program;

public static class ServicesExtensions
{
    public static IServiceCollection AddUtils(this IServiceCollection services)
    {
        services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
        services.AddSingleton<IRequestClient, HttpRequestClient>();

        services.AddHttpContextAccessor();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<ITopicRepository, TopicRepository>();

        return services;
    }

    public static IServiceCollection AddSubServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(configuration.GetSection("Auth0").Get<Auth0Options>());
        services.AddSingleton<IExternalAuthRepository, Auth0AuthRepository>();
        services.AddSingleton<IDataMemoryService, AspNetDataMemoryService>();
        services.AddSingleton<INotificationSender, SignalRNotificationSender>();
        
        services.AddScoped<ILoadInfoWorker, LoadInfoWorker>();
        services.AddScoped<ILoadTopTopicsWorker, LoadTopTopicsWorker>();

        return services;
    }

    public static IServiceCollection AddMainServices(this IServiceCollection services)
    {
        services.AddHostedService<BackgroundTaskManager>();
        services.AddHostedService<LoadTopTopicsTimedService>();

        return services;
    }
}
