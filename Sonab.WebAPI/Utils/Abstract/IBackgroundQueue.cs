using Sonab.WebAPI.Models;

namespace Sonab.WebAPI.Utils.Abstact;

public interface IBackgroundQueue
{
    void Enqueue(JobInfo job);
    Task<JobInfo> Dequeue();
}
