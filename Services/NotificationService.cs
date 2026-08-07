using Plugin.LocalNotification;

namespace StudentTaskManager.Services;

public class NotificationService
{
    public async Task ScheduleNotification(
        int id,
        string title,
        string description,
        DateTime notifyTime)
    {
        var request = new NotificationRequest
        {
            NotificationId = id,
            Title = title,
            Description = description,
            Schedule = new NotificationRequestSchedule
            {
                NotifyTime = notifyTime
            }
        };

        await LocalNotificationCenter.Current.Show(request);
    }
}