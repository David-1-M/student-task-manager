using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudentTaskManager.Models;
using StudentTaskManager.Services;

namespace StudentTaskManager.ViewModels;

public partial class RegisterViewModel : ObservableObject
{
    private readonly AuthenticationService _authentication;

    public RegisterViewModel(
        AuthenticationService authentication)
    {
        _authentication = authentication;
    }

    [ObservableProperty]
    private string fullName = string.Empty;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private string confirmPassword = string.Empty;

    [RelayCommand]
    private async Task Register()
    {
        // Validate required fields
        if (string.IsNullOrWhiteSpace(FullName) ||
            string.IsNullOrWhiteSpace(Email) ||
            string.IsNullOrWhiteSpace(Password) ||
            string.IsNullOrWhiteSpace(ConfirmPassword))
        {
            await Shell.Current.DisplayAlert(
                "Missing Information",
                "Please complete all fields.",
                "OK");

            return;
        }

        // Basic email validation
        if (!Email.Contains("@") ||
            !Email.Contains("."))
        {
            await Shell.Current.DisplayAlert(
                "Invalid Email",
                "Please enter a valid email address.",
                "OK");

            return;
        }

        // Password length
        if (Password.Length < 6)
        {
            await Shell.Current.DisplayAlert(
                "Weak Password",
                "Your password must contain at least 6 characters.",
                "OK");

            return;
        }

        // Confirm password
        if (Password != ConfirmPassword)
        {
            await Shell.Current.DisplayAlert(
                "Password Mismatch",
                "The passwords do not match.",
                "OK");

            return;
        }

        User user = new()
        {
            FullName = FullName.Trim(),
            Email = Email.Trim().ToLowerInvariant(),
            Password = Password
        };

        bool success =
            await _authentication.Register(user);

        if (!success)
        {
            await Shell.Current.DisplayAlert(
                "Registration Failed",
                "An account with this email already exists.",
                "OK");

            return;
        }

        await Shell.Current.DisplayAlert(
            "Registration Successful",
            "Your account has been created successfully.",
            "OK");

        // Return to Login
        await Shell.Current.GoToAsync("..");
    }
}