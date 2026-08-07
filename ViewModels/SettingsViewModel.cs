using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudentTaskManager.Views;

namespace StudentTaskManager.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    [ObservableProperty]
    private string userName = "Student";

    [ObservableProperty]
    private string userEmail = "Not available";

    public SettingsViewModel()
    {
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

        await Shell.Current.GoToAsync(nameof(LoginPage));
    }
}