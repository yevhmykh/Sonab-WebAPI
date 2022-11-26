using System.Collections.Concurrent;
using Sonab.WebAPI.Models;
using Sonab.WebAPI.Utils.Abstact;

namespace Sonab.WebAPI.Utils.Queue;

public class BackgroundQueue : IBackgroundQueue
{
    private readonly ConcurrentQueue<JobInfo> _items = new();
    private readonly SemaphoreSlim _semaphore = new(0, int.MaxValue);

    public void Enqueue(JobInfo job)
    {
        _items.Enqueue(job);
        _semaphore.Release();
    }

    public async Task<JobInfo> Dequeue()
    {
        await _semaphore.WaitAsync();

        return _items.TryDequeue(out JobInfo workItem) ?
            workItem :
            throw new InvalidOperationException("Job is not dequered");
    }
}

