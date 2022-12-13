using Sonab.WebAPI.Enums;

namespace Sonab.WebAPI.Models.Notifications;

public struct Notification
{
    public object Data { get; set; }
    public NotificationType Type { get; set; }

    public Notification(NotificationType type, object data = null)
    {
        Type = type;
        Data = data;
    }
}
