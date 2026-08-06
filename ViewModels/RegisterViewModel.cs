using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudentTaskManager.Models;
using StudentTaskManager.Services;

namespace StudentTaskManager.ViewModels;

public partial class RegisterViewModel : ObservableObject
{
    private readonly AuthenticationService _authentication;

    public RegisterViewModel(AuthenticationService authentication)
    {
        _authentication = authentication;
    }

    [ObservableProperty]
    private string fullName = "";

    [ObservableProperty]
    private string email = "";

    [ObservableProperty]
    private string password = "";

    [ObservableProperty]
    private string confirmPassword = "";

    [RelayCommand]
    private async Task Register()
    {
        if (Password != ConfirmPassword)
        {
            await Shell.Current.DisplayAlert(
                "Error",
                "Passwords do not match.",
                "OK");

            return;
        }

        User user = new()
        {
            FullName = FullName,
            Email = Email,
            Password = Password
        };

        bool success =
            await _authentication.Register(user);

        if (!success)
        {
            await Shell.Current.DisplayAlert(
                "Error",
                "Email already exists.",
                "OK");

            return;
        }

        await Shell.Current.DisplayAlert(
            "Success",
            "Account created successfully.",
            "OK");

        await Shell.Current.GoToAsync("..");
    }
}