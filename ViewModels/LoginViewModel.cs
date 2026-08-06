using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudentTaskManager.Services;
using StudentTaskManager.Views;

namespace StudentTaskManager.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    private readonly AuthenticationService _authentication;

    public LoginViewModel(AuthenticationService authentication)
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
                "Error",
                "Enter your email and password.",
                "OK");

            return;
        }

        var user = await _authentication.Login(Email, Password);

        if (user == null)
        {
            await Shell.Current.DisplayAlert(
                "Login Failed",
                "Incorrect email or password.",
                "OK");

            return;
        }

        await Shell.Current.GoToAsync(nameof(HomePage));
    }

    [RelayCommand]
    private async Task Register()
    {
        await Shell.Current.GoToAsync(nameof(RegisterPage));
    }
}