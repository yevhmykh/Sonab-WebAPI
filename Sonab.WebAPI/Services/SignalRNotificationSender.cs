using Microsoft.AspNetCore.SignalR;
using Sonab.Core.Interfaces.Services;
using Sonab.WebAPI.Enums;
using Sonab.WebAPI.Hubs;
using Sonab.WebAPI.Models.Notifications;

namespace Sonab.WebAPI.Services;

public class SignalRNotificationSender : INotificationSender
{
    private readonly IHubContext<NotificationHub> _hub;

    public SignalRNotificationSender(IHubContext<NotificationHub> hub)
    {
        _hub = hub;
    }

    public Task SendErrorAsync(string userId, string message) =>
        _hub.Clients.User(userId)
            .SendAsync("messageReceived", new Notification(NotificationType.Error, message));
}
