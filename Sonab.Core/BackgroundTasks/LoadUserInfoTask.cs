namespace Sonab.Core.BackgroundTasks;

public record LoadUserInfoTask(string ExternalUserId) : BackgroundTask;