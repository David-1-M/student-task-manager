using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudentTaskManager.Models;
using StudentTaskManager.Services;

namespace StudentTaskManager.ViewModels;

public partial class AddTaskViewModel : ObservableObject
{
    private readonly DatabaseService _database;
    private readonly NotificationService _notificationService;

    [ObservableProperty]
    private string title = string.Empty;

    [ObservableProperty]
    private string description = string.Empty;

    [ObservableProperty]
    private string category = string.Empty;

    [ObservableProperty]
    private string priority = string.Empty;

    [ObservableProperty]
    private DateTime dueDate = DateTime.Today;

    [ObservableProperty]
    private TimeSpan dueTime = DateTime.Now.AddHours(1).TimeOfDay;

    public List<string> Categories { get; } = new()
    {
        "Assignments",
        "Projects",
        "Tests",
        "Exams",
        "Personal",
        "Other"
    };

    public List<string> Priorities { get; } = new()
    {
        "Low",
        "Medium",
        "High"
    };

    public AddTaskViewModel(
        DatabaseService database,
        NotificationService notificationService)
    {
        _database = database;
        _notificationService = notificationService;
    }

    [RelayCommand]
    private async Task SaveTask()
    {
        // Validate logged-in user
        int userId = Preferences.Default.Get(
            "LoggedInUserId",
            0);

        if (userId == 0)
        {
            await Shell.Current.DisplayAlert(
                "Not Logged In",
                "Your session has expired. Please log in again.",
                "OK");

            await Shell.Current.GoToAsync(
                "//" + nameof(Views.LoginPage));

            return;
        }

        // Validate title
        if (string.IsNullOrWhiteSpace(Title))
        {
            await Shell.Current.DisplayAlert(
                "Missing Title",
                "Please enter a task title.",
                "OK");

            return;
        }

        // Validate description
        if (string.IsNullOrWhiteSpace(Description))
        {
            await Shell.Current.DisplayAlert(
                "Missing Description",
                "Please enter a task description.",
                "OK");

            return;
        }

        // Validate category
        if (string.IsNullOrWhiteSpace(Category))
        {
            await Shell.Current.DisplayAlert(
                "Missing Category",
                "Please select a task category.",
                "OK");

            return;
        }

        // Validate priority
        if (string.IsNullOrWhiteSpace(Priority))
        {
            await Shell.Current.DisplayAlert(
                "Missing Priority",
                "Please select a task priority.",
                "OK");

            return;
        }

        // Combine date and time
        DateTime dueDateTime = DueDate.Date.Add(DueTime);

        // Validate due date/time
        if (dueDateTime <= DateTime.Now)
        {
            await Shell.Current.DisplayAlert(
                "Invalid Due Date",
                "Please select a future date and time.",
                "OK");

            return;
        }

        // Create task
        var task = new TaskItem
        {
            UserId = userId,
            Title = Title.Trim(),
            Description = Description.Trim(),
            Category = Category,
            Priority = Priority,
            DueDate = dueDateTime,
            IsCompleted = false
        };

        // Save task
        int result = await _database.AddTaskAsync(task);

        if (result <= 0)
        {
            await Shell.Current.DisplayAlert(
                "Error",
                "The task could not be saved. Please try again.",
                "OK");

            return;
        }

        // Schedule notification
        await ScheduleNotificationAsync(task);

        await Shell.Current.DisplayAlert(
            "Task Added",
            "Your task has been added successfully.",
            "OK");

        await Shell.Current.GoToAsync("..");
    }

    private async Task ScheduleNotificationAsync(TaskItem task)
    {
        string preference = Preferences.Default.Get(
            $"NotificationPreference_{task.UserId}",
            "1 hour before");

        if (preference == "Notifications Off")
            return;

        TimeSpan offset = preference switch
        {
            "15 minutes before" => TimeSpan.FromMinutes(15),
            "30 minutes before" => TimeSpan.FromMinutes(30),
            "1 hour before" => TimeSpan.FromHours(1),
            "1 day before" => TimeSpan.FromDays(1),
            "2 days before" => TimeSpan.FromDays(2),
            _ => TimeSpan.FromHours(1)
        };

        DateTime notificationTime = task.DueDate - offset;

        if (notificationTime <= DateTime.Now)
            return;

        await _notificationService.ScheduleTaskReminderAsync(
            task.Id,
            task.Title,
            $"Due at {task.DueDate:dd MMM yyyy HH:mm}",
            notificationTime);
    }
}