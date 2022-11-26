using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sonab.WebAPI.Enums;
using Sonab.WebAPI.Models;
using Sonab.WebAPI.Services;
using Sonab.WebAPI.Services.Workers.Abstract;
using Sonab.WebAPI.Utils.Abstact;

namespace Sonab.WebAPI.Test.Services;

public class BackgroundWorkerTests
{
    private readonly Mock<IServiceScopeFactory> _mockScopeFactory = new();
    private readonly Mock<IServiceScope> _mockServiceScope = new();
    private readonly Mock<IServiceProvider> _mockServiceProvider = new();
    private readonly Mock<IBackgroundQueue> _mockQueue = new();
    private readonly BackgroundWorker _worker;

    public BackgroundWorkerTests()
    {
        _mockServiceScope.Setup(x => x.ServiceProvider).Returns(_mockServiceProvider.Object);
        _mockScopeFactory.Setup(x => x.CreateScope()).Returns(_mockServiceScope.Object);

        _worker = new(
            Mock.Of<ILogger<BackgroundWorker>>(),
            _mockQueue.Object,
            _mockScopeFactory.Object);
    }

    [Fact]
    public async Task RunAndStop()
    {
        // Setup
        int count = 0;
        var tokenSource = new CancellationTokenSource();
        var mockWorker = new Mock<ILoadInfoWorker>();
        mockWorker.Setup(x => x.StartWork(It.Is<object>(y => (string)y == "1"), It.IsAny<CancellationToken>())).Callback(() =>
        {
            count++;
            Thread.Sleep(1000);
        });
        _mockQueue.Setup(x => x.Dequeue()).ReturnsAsync(new JobInfo(JobType.LoadInfo, "1"));
        _mockServiceProvider.Setup(x => x.GetService(It.IsAny<Type>())).Returns(mockWorker.Object);

        // Act
        var task = Task.Run(() => _worker.StartAsync(tokenSource.Token));
        Thread.Sleep(100);
        tokenSource.Cancel();
        await task;

        // Assert
        Assert.Equal(1, count);
    }
}
