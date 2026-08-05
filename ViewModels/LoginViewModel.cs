using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


namespace StudentTaskManager.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [RelayCommand]
    private async Task Login()
    {
        await Application.Current.MainPage.DisplayAlert(
            "Login",
            $"Welcome {Email}",
            "OK");
    }

    [RelayCommand]
    private async Task Register()
    {
        await Shell.Current.GoToAsync("RegisterPage");
    }
}