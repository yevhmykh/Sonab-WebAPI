using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Sonab.Core.Dto.Posts;
using Sonab.Core.Dto.TopicTags;
using Sonab.Core.Interfaces.Repositories;
using Sonab.Core.Interfaces.Repositories.ReadEntity;
using Sonab.WebAPI.Repositories.Abstract;
using Sonab.WebAPI.Services.Background.Workers;

namespace Sonab.WebAPI.Test.Services.Workers;

public class LoadTopTopicsWorkerTests
{
    private readonly Mock<ITopicRepository> _mockTopicRepository = new();
    private readonly Mock<IMemoryCache> _mockCache = new();
    private readonly LoadTopTopicsWorker _worker;

    public LoadTopTopicsWorkerTests()
    {
        _worker = new(
            Mock.Of<ILogger<LoadTopTopicsWorker>>(),
            _mockCache.Object,
            _mockTopicRepository.Object
        );
    }

    [Fact]
    public async Task LoadTop_Ok()
    {
        // Setup
        List<TopicTag> tags = new()
        {
            new TopicTag
            {
                Name = "Some"
            },
            new TopicTag
            {
                Name = "Booo"
            },
            new TopicTag
            {
                Name = "Another"
            }
        };
        _mockTopicRepository.Setup(x => x.GetTopAsync())
            .ReturnsAsync(tags);
        bool isSaved = false;
        _mockCache.Setup(x => x.CreateEntry(It.IsAny<object>()))
            .Callback(() => isSaved = true)
            .Returns(Mock.Of<ICacheEntry>());

        // Act
        await _worker.StartWork(null, new CancellationToken());

        // Assert
        Assert.True(isSaved);
    }
}
