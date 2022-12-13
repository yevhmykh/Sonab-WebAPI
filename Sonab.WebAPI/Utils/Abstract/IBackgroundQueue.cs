using Sonab.WebAPI.Models.Jobs;

namespace Sonab.WebAPI.Utils.Abstact;

public interface IBackgroundQueue
{
    void Enqueue(JobInfo job);
    Task<JobInfo> Dequeue();
}
