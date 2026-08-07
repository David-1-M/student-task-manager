using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudentTaskManager.Services;
using StudentTaskManager.Views;

namespace StudentTaskManager.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly AuthenticationService _authentication;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    public LoginViewModel(
        AuthenticationService authentication)
    {
        _authentication = authentication;
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

        // Clear password after successful login
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