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
    }

    // TEST NOTIFICATION
    [RelayCommand]
    private async Task TestNotification()
    {
        bool enabled = await _notificationService.AreNotificationsEnabledAsync();

        if (!enabled)
        {
            bool granted = await _notificationService.RequestPermissionAsync();

            if (!granted)
            {
                await Shell.Current.DisplayAlert(
                    "Notifications Disabled",
                    "Please allow notifications for Student Task Manager in your phone's settings.",
                    "OK");

                return;
            }
        }

        await _notificationService.SendTestNotificationAsync();
    }

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