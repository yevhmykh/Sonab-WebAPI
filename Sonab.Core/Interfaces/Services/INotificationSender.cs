namespace Sonab.Core.Interfaces.Services;

public interface INotificationSender
{
    Task SendErrorAsync(string userId, string message);
}
