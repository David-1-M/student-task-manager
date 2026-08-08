using Plugin.LocalNotification;

namespace StudentTaskManager.Services;

public class NotificationService
{
    public async Task<bool> RequestPermissionAsync()
    {
#if WINDOWS
        return false;
#else
        return await LocalNotificationCenter.Current
            .RequestNotificationPermission();
#endif
    }

    public async Task<bool> AreNotificationsEnabledAsync()
    {
#if WINDOWS
        return false;
#else
        return await LocalNotificationCenter.Current
            .AreNotificationsEnabled();
#endif
    }

    public async Task SendTestNotificationAsync()
    {
#if WINDOWS
        await Task.CompletedTask;
#else
        var request = new NotificationRequest
        {
            NotificationId = 999999,
            Title = "Student Task Manager",
            Description = "Notifications are working!"
        };

        await LocalNotificationCenter.Current.Show(request);
#endif
    }

    public async Task ScheduleTaskReminderAsync(
        int taskId,
        string title,
        string description,
        DateTime notifyTime)
    {
#if WINDOWS
        await Task.CompletedTask;
#else
        if (notifyTime <= DateTime.Now)
            return;

        var request = new NotificationRequest
        {
            NotificationId = taskId,
            Title = $"Task Reminder: {title}",
            Description = description,
            Schedule = new NotificationRequestSchedule
            {
                NotifyTime = notifyTime
            }
        };

        await LocalNotificationCenter.Current.Show(request);
#endif
    }

    public void CancelTaskReminder(int taskId)
    {
#if WINDOWS
        return;
#else
        LocalNotificationCenter.Current.Cancel(taskId);
#endif
    }
}