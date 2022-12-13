using Sonab.WebAPI.Enums;

namespace Sonab.WebAPI.Models.Jobs;

public struct JobInfo
{
    public object Data { get; set; }
    public JobType Type { get; set; }

    public JobInfo(JobType type, object data = null)
    {
        Type = type;
        Data = data;
    }
}
