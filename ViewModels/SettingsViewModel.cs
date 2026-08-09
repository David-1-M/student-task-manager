using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudentTaskManager.Services;
using StudentTaskManager.Views;

namespace StudentTaskManager.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private readonly NotificationService _notificationService;

    [ObservableProperty]
    private string userName = "Student";

    [ObservableProperty]
    private string userEmail = "Not available";

    [ObservableProperty]
    private string notificationPreference = "1 hour before";

    public List<string> NotificationOptions { get; } =
    [
        "Notifications Off",
        "15 minutes before",
        "30 minutes before",
        "1 hour before",
        "1 day before",
        "2 days before"
    ];

    public SettingsViewModel(NotificationService notificationService)
    {
        _notificationService = notificationService;

        LoadUserInformation();
    }

    private void LoadUserInformation()
    {
        UserName = Preferences.Get(
            "LoggedInUserName",
            "Student");

        UserEmail = Preferences.Get(
            "LoggedInUserEmail",
            "Not available");

        NotificationPreference = Preferences.Get(
            GetNotificationPreferenceKey(),
            "1 hour before");
    }

    partial void OnNotificationPreferenceChanged(string value)
    {
        Preferences.Set(
            GetNotificationPreferenceKey(),
            value);

        // Request notification permission when notifications are enabled.
        if (value != "Notifications Off")
        {
            _ = RequestNotificationPermissionAsync();
        }
    }

    private async Task RequestNotificationPermissionAsync()
    {
        bool permissionGranted =
            await _notificationService.RequestPermissionAsync();

        if (!permissionGranted)
        {
            await Shell.Current.DisplayAlert(
                "Notifications Disabled",
                "Please allow notifications for Student Task Manager in your phone's settings.",
                "OK");
        }
    }

    private static string GetNotificationPreferenceKey()
    {
        int userId = Preferences.Get(
            "LoggedInUserId",
            0);

        return $"NotificationPreference_{userId}";
    }

    // -------------------------
    // TEST NOTIFICATION
    // -------------------------

    [RelayCommand]
    private async Task TestNotification()
    {
        bool permissionGranted =
            await _notificationService.RequestPermissionAsync();

        if (!permissionGranted)
        {
            await Shell.Current.DisplayAlert(
                "Notifications Disabled",
                "Please allow notifications for Student Task Manager in your device settings.",
                "OK");

            return;
        }

        await _notificationService.SendTestNotificationAsync();

        await Shell.Current.DisplayAlert(
            "Notification Sent",
            "A test notification has been sent. Check your notification panel.",
            "OK");
    }

    // -------------------------
    // LOGOUT
    // -------------------------

    [RelayCommand]
    private async Task Logout()
    {
        bool answer = await Shell.Current.DisplayAlert(
            "Logout",
            "Are you sure you want to logout?",
            "Yes",
            "No");

        if (!answer)
            return;

        Preferences.Remove("LoggedInUserId");
        Preferences.Remove("LoggedInUserName");
        Preferences.Remove("LoggedInUserEmail");

        await Shell.Current.GoToAsync("//Login");
    }
}