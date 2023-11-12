using Microsoft.AspNetCore.SignalR;
using Sonab.WebAPI.Enums;
using Sonab.WebAPI.Models.Notifications;

namespace Sonab.WebAPI.Extensions;

public static class NotificationExtentions
{
    public static Task SendAsync(this IClientProxy client, NotificationType type, object data = null) =>
        client.SendAsync("messageReceived", new Notification(type, data));

    public static Task SendErrorAsync(this IClientProxy client, string message) =>
        client.SendAsync("messageReceived", new Notification(NotificationType.Error, message));
}
