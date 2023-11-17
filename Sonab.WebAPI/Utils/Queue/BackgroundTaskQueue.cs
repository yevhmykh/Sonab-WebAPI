using System.Collections.Concurrent;
using Sonab.Core.BackgroundTasks;
using Sonab.Core.Interfaces.Services;

namespace Sonab.WebAPI.Utils.Queue;

public class BackgroundTaskQueue : IBackgroundTaskQueue
{
    private readonly ConcurrentQueue<BackgroundTask> _items = new();
    private readonly SemaphoreSlim _semaphore = new(0, int.MaxValue);

    public void Enqueue(BackgroundTask task)
    {
        _items.Enqueue(task);
        _semaphore.Release();
    }

    public async Task<BackgroundTask> Dequeue()
    {
        await _semaphore.WaitAsync();

        return _items.TryDequeue(out BackgroundTask workItem)
            ? workItem
            : throw new InvalidOperationException("Task is not dequeued");
    }
}
