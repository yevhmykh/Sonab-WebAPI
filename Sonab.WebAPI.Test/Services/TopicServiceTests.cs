using Microsoft.Extensions.Logging;
using Sonab.WebAPI.Models;
using Sonab.WebAPI.Models.Posts;
using Sonab.WebAPI.Repositories.Abstract;
using Sonab.WebAPI.Services;

namespace Sonab.WebAPI.Test.Services;

public class TopicServiceTests : BaseServiceSetup
{
    private readonly Mock<ITopicRepository> _mockRepository = new();
    private readonly Mock<Microsoft.Extensions.Caching.Memory.IMemoryCache> _mockCache = new();
    private readonly TopicService _service;

    public TopicServiceTests()
    {
        _service = new(
            Mock.Of<ILogger<TopicService>>(),
            _mockCache.Object,
            _mockRepository.Object);
    }

    [Fact]
    public async Task GetTop_FromCache()
    {
        // Setup
        object tags = Tags;
        _mockCache.Setup(x => x.TryGetValue(It.IsAny<object>(), out tags))
            .Verifiable();
        bool isRepositoryCalled = false;
        _mockRepository.Setup(x => x.GetTopAsync())
            .Callback(() => isRepositoryCalled = true)
            .ReturnsAsync(Tags);

        // Act
        ServiceResponse result = await _service.GetListAsync(null);

        // Assert
        Assert.False(isRepositoryCalled);
        Assert.True(result.TryGetData(out List<TopicTag> data));
        Assert.Equal(2, data.Count);
    }

    [Fact]
    public async Task GetTop_FromDb()
    {
        // Setup
        object tags = null as List<TopicTag>;
        bool isCacheCalled = false;
        _mockCache.Setup(x => x.TryGetValue(It.IsAny<object>(), out tags))
            .Callback(() => isCacheCalled = true)
            .Returns(false);
        bool isRepositoryCalled = false;
        _mockRepository.Setup(x => x.GetTopAsync())
            .Callback(() => isRepositoryCalled = true)
            .ReturnsAsync(Tags);

        // Act
        ServiceResponse result = await _service.GetListAsync(null);

        // Assert
        Assert.True(isCacheCalled);
        Assert.True(isRepositoryCalled);
        Assert.True(result.TryGetData(out List<TopicTag> data));
        Assert.Equal(2, data.Count);
    }

    private static List<TopicTag> Tags => new()
    {
        new TopicTag
        {
            Name = "Some"
        },
        new TopicTag
        {
            Name = "Booo"
        }
    };
}
