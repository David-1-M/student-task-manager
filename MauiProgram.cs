using CommunityToolkit.Maui;
using StudentTaskManager.Services;
using StudentTaskManager.ViewModels;
using StudentTaskManager.Views;

namespace StudentTaskManager;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Services
        builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddSingleton<AuthenticationService>();

        // ViewModels
        builder.Services.AddSingleton<LoginViewModel>();
        builder.Services.AddTransient<RegisterViewModel>();

        // Pages
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<AddTaskPage>();
        builder.Services.AddTransient<EditTaskPage>();

        return builder.Build();
    }
}