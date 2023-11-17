using Sonab.Core.Dto.TopicTags.Requests;
using Sonab.Core.Dto.TopicTags.Responses;
using Sonab.Core.Errors;
using Sonab.Core.Interfaces;
using Sonab.Core.Interfaces.Repositories.ReadEntity;
using Sonab.Core.Interfaces.Services;
using Sonab.Core.UseCases.TopicTags;
using Sonab.WebAPI.Services.Background.Workers.Abstract;

namespace Sonab.WebAPI.Services.Background.Workers;

public sealed class LoadTopTopicsWorker : ILoadTopTopicsWorker
{
    private readonly ILogger<LoadTopTopicsWorker> _logger;
    private readonly IDataMemoryService _dataMemoryService;
    private readonly ITopicRepository _topicRepository;

    public LoadTopTopicsWorker(
        ILogger<LoadTopTopicsWorker> logger,
        IDataMemoryService dataMemoryService,
        ITopicRepository topicRepository)
    {
        _logger = logger;
        _dataMemoryService = dataMemoryService;
        _topicRepository = topicRepository;
    }

    public async Task StartWork(object data, CancellationToken stoppingToken)
    {
        _logger.LogDebug("Loading most popular topic tags");

        GetTopTopicTagsUseCase useCase = new(_dataMemoryService, _topicRepository);
        Presenter presenter = new();
        await useCase.Handle(null, new GetTopicTagsRequest(null), presenter);
        
        _logger.LogDebug($"Cached {presenter.TagCount} tags.");
    }
    
    
    private class Presenter : IPresenter<GetTopicTagsResponse>
    {
        public int TagCount { get; private set; }
        public Task HandleSuccess(GetTopicTagsResponse response)
        {
            TagCount = response.TopicTags.Count;
            return Task.CompletedTask;
        }

        public Task HandleFailure(ErrorBase error) => throw new InvalidOperationException();
    }
}
