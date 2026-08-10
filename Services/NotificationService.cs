using Plugin.LocalNotification;

namespace StudentTaskManager.Services;

public class NotificationService
{
    public async Task<bool> RequestPermissionAsync()
    {
#if WINDOWS
        return false;
#else
        try
        {
            return await LocalNotificationCenter.Current
                .RequestNotificationPermission();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(
                $"Notification permission error: {ex}");

            return false;
        }
#endif
    }

    public async Task<bool> AreNotificationsEnabledAsync()
    {
#if WINDOWS
        return false;
#else
        try
        {
            return await LocalNotificationCenter.Current
                .AreNotificationsEnabled();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(
                $"Notification status error: {ex}");

            return false;
        }
#endif
    }

    public async Task SendTestNotificationAsync()
    {
#if WINDOWS
        await Task.CompletedTask;
#else
        try
        {
            var request = new NotificationRequest
            {
                NotificationId = 999999,
                Title = "Student Task Manager",
                Description = "Notifications are working!"
            };

            await LocalNotificationCenter.Current.Show(request);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(
                $"Test notification error: {ex}");
        }
#endif
    }

    public async Task<bool> ScheduleTaskReminderAsync(
        int taskId,
        string title,
        string description,
        DateTime notifyTime)
    {
#if WINDOWS
        return false;
#else
        try
        {
            if (notifyTime <= DateTime.Now)
                return false;

            bool notificationsEnabled =
                await AreNotificationsEnabledAsync();

            if (!notificationsEnabled)
            {
                System.Diagnostics.Debug.WriteLine(
                    "Notifications are disabled.");

                return false;
            }

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

            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(
                $"Schedule notification error: {ex}");

            return false;
        }
#endif
    }

    public void CancelTaskReminder(int taskId)
    {
#if WINDOWS
        return;
#else
        try
        {
            LocalNotificationCenter.Current.Cancel(taskId);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(
                $"Cancel notification error: {ex}");
        }
#endif
    }
}