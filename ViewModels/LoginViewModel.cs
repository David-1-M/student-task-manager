using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudentTaskManager.Services;

namespace StudentTaskManager.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    private readonly AuthenticationService _authenticationService;

    public LoginViewModel(AuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;

        Title = "Login";
    }

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (IsBusy)
            return;

        IsBusy = true;

        try
        {
            var user = await _authenticationService.LoginAsync(Email, Password);

            if (user == null)
            {
                await Shell.Current.DisplayAlert(
                    "Login Failed",
                    "Invalid email or password.",
                    "OK");

                return;
            }

            await Shell.Current.DisplayAlert(
                "Welcome",
                $"Welcome back, {user.Name}!",
                "Continue");

            // We'll navigate to the Home Page later.
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task GoToRegisterAsync()
    {
        await Shell.Current.GoToAsync("//RegisterPage");
    }
}