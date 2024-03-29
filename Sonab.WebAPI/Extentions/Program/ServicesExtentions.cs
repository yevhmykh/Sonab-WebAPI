using Sonab.WebAPI.Repositories;
using Sonab.WebAPI.Repositories.Abstract;
using Sonab.WebAPI.Services;
using Sonab.WebAPI.Services.Abstract;
using Sonab.WebAPI.Services.Auth0Communication;
using Sonab.WebAPI.Services.Background.Timed;
using Sonab.WebAPI.Services.Background.Workers;
using Sonab.WebAPI.Services.Background.Workers.Abstract;
using Sonab.WebAPI.Utils.Abstact;
using Sonab.WebAPI.Utils.Queue;
using Sonab.WebAPI.Utils.RequestClients;

namespace Sonab.WebAPI.Extentions.Program;

public static class ServicesExtentions
{
    public static IServiceCollection AddUtils(this IServiceCollection services)
    {
        services.AddSingleton<IBackgroundQueue, BackgroundQueue>();
        services.AddSingleton<IRequestClient, HttpRequestClient>();

        services.AddHttpContextAccessor();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<ITopicRepository, TopicRepository>();

        return services;
    }

    public static IServiceCollection AddSubServices(this IServiceCollection services)
    {
        services.AddScoped<IAuth0CommunicationService, Auth0CommunicationService>();
        
        services.AddScoped<ILoadInfoWorker, LoadInfoWorker>();
        services.AddScoped<ILoadTopTopicsWorker, LoadTopTopicsWorker>();

        return services;
    }

    public static IServiceCollection AddMainServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ISubsriptionService, SubsriptionService>();
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<ITopicService, TopicService>();

        services.AddHostedService<BackgroundJobManager>();
        services.AddHostedService<LoadTopTopicsTimedService>();

        return services;
    }
}
