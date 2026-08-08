using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudentTaskManager.Services;
using StudentTaskManager.Views;

namespace StudentTaskManager.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly AuthenticationService _authentication;
    private readonly DatabaseService _database;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    public LoginViewModel(
        AuthenticationService authentication,
        DatabaseService database)
    {
        _authentication = authentication;
        _database = database;
    }

    [RelayCommand]
    private async Task Login()
    {
        if (string.IsNullOrWhiteSpace(Email) ||
            string.IsNullOrWhiteSpace(Password))
        {
            await Shell.Current.DisplayAlert(
                "Missing Information",
                "Please enter your email and password.",
                "OK");

            return;
        }

        var user =
            await _authentication.Login(
                Email,
                Password);

        if (user == null)
        {
            await Shell.Current.DisplayAlert(
                "Login Failed",
                "Incorrect email or password.",
                "OK");

            return;
        }

        // Save logged in user
        Preferences.Set("LoggedInUserId", user.Id);
        Preferences.Set("LoggedInUserName", user.FullName);
        Preferences.Set("LoggedInUserEmail", user.Email);

        // Claim legacy tasks created before multi-user support
        await _database.AssignLegacyTasksToUserAsync(user.Id);

        Password = string.Empty;

        await Shell.Current.GoToAsync(
            nameof(HomePage));
    }

    [RelayCommand]
    private async Task Register()
    {
        await Shell.Current.GoToAsync(
            nameof(RegisterPage));
    }
}