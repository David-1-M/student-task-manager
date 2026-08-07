using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudentTaskManager.Models;
using StudentTaskManager.Services;

namespace StudentTaskManager.ViewModels;

public partial class AddTaskViewModel : ObservableObject
{
    private readonly DatabaseService _database;

    [ObservableProperty]
    private string title = string.Empty;

    [ObservableProperty]
    private string description = string.Empty;

    [ObservableProperty]
    private string category = "School";

    [ObservableProperty]
    private DateTime dueDate = DateTime.Today;

    [ObservableProperty]
    private string priority = "Medium";

    private readonly NotificationService _notifications;

    public AddTaskViewModel(
    DatabaseService database,
    NotificationService notifications)
    {
        _database = database;
        _notifications = notifications;
    }

    [RelayCommand]
    private async Task SaveTask()
    {
        if (string.IsNullOrWhiteSpace(Title))
        {
            await Shell.Current.DisplayAlert(
                "Error",
                "Task title is required.",
                "OK");

            return;
        }

        var task = new TaskItem
        {
            Title = Title,
            Description = Description,
            Category = Category,
            DueDate = DueDate,
            Priority = Priority,
            IsCompleted = false
        };

        await _database.AddTaskAsync(task);

        await Shell.Current.GoToAsync("..");

        await _notifications.ScheduleNotification(
            task.Id,
            task.Title,
            "Task deadline approaching!",
            task.DueDate.AddHours(-24));
    }

    public List<string> Categories { get; } =
    new()
    {
        "School",
        "Work",
        "Assignments",
        "Projects",
        "Tests",
        "Meetings",
        "Personal",
        "Shopping",
        "Health",
        "Finance",
        "Other"
    };

    public List<string> Priorities { get; } =
    new()
    {
        "High",
        "Medium",
        "Low"
    };
}