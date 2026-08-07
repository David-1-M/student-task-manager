using Plugin.LocalNotification;

namespace StudentTaskManager.Services;

public class NotificationService
{
    public async Task SendTaskReminderAsync(
        int taskId,
        string title,
        string description)
    {
#if WINDOWS
        // Local notifications are not currently supported
        // by this implementation on Windows.
        await Task.CompletedTask;
#else
        var request = new NotificationRequest
        {
            NotificationId = taskId,
            Title = title,
            Description = description
        };

        await LocalNotificationCenter.Current.Show(request);
#endif
    }
}