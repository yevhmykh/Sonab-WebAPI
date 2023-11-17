using Sonab.Core.BackgroundTasks;

namespace Sonab.Core.Interfaces.Services;

public interface IBackgroundTaskQueue
{
    void Enqueue(BackgroundTask task);
    Task<BackgroundTask> Dequeue();
}
