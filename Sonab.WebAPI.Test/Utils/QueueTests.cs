using Sonab.WebAPI.Enums;
using Sonab.WebAPI.Models;
using Sonab.WebAPI.Utils.Queue;

namespace Sonab.WebAPI.Test.Utils;

public class QueueTests
{
    [Fact]
    public async Task OneJob()
    {
        // Setup
        BackgroundQueue backgroundQueue = new();
        backgroundQueue.Enqueue(new JobInfo(JobType.LoadInfo, "1"));

        // Act
        JobInfo result = await backgroundQueue.Dequeue();

        // Assert
        Assert.Equal(JobType.LoadInfo, result.Type);
        Assert.Equal("1", (string)result.Data);
    }
}
