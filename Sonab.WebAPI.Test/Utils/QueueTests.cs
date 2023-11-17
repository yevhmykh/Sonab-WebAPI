using Sonab.Core.BackgroundTasks;
using Sonab.WebAPI.Utils.Queue;

namespace Sonab.WebAPI.Test.Utils;

public class QueueTests
{
    [Fact]
    public async Task OneJob()
    {
        // Setup
        BackgroundTaskQueue backgroundTaskQueue = new();
        backgroundTaskQueue.Enqueue(new LoadUserInfoTask("1"));

        // Act
        BackgroundTask result = await backgroundTaskQueue.Dequeue();

        // Assert
        Assert.IsType<LoadUserInfoTask>(result);
        Assert.Equal("1", ((LoadUserInfoTask)result).ExternalUserId);
    }
}
