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
    private string category = string.Empty;

    [ObservableProperty]
    private string priority = string.Empty;

    [ObservableProperty]
    private DateTime dueDate = DateTime.Today;

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

    public AddTaskViewModel(DatabaseService database)
    {
        _database = database;
    }

    [RelayCommand]
    private async Task SaveTask()
    {
        // -------------------------
        // VALIDATE USER
        // -------------------------

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

        // -------------------------
        // VALIDATE TITLE
        // -------------------------

        if (string.IsNullOrWhiteSpace(Title))
        {
            await Shell.Current.DisplayAlert(
                "Missing Title",
                "Please enter a task title.",
                "OK");

            return;
        }

        // -------------------------
        // VALIDATE DESCRIPTION
        // -------------------------

        if (string.IsNullOrWhiteSpace(Description))
        {
            await Shell.Current.DisplayAlert(
                "Missing Description",
                "Please enter a task description.",
                "OK");

            return;
        }

        // -------------------------
        // VALIDATE CATEGORY
        // -------------------------

        if (string.IsNullOrWhiteSpace(Category))
        {
            await Shell.Current.DisplayAlert(
                "Missing Category",
                "Please select a task category.",
                "OK");

            return;
        }

        // -------------------------
        // VALIDATE PRIORITY
        // -------------------------

        if (string.IsNullOrWhiteSpace(Priority))
        {
            await Shell.Current.DisplayAlert(
                "Missing Priority",
                "Please select a task priority.",
                "OK");

            return;
        }

        // -------------------------
        // VALIDATE DATE
        // -------------------------

        if (DueDate.Date < DateTime.Today)
        {
            await Shell.Current.DisplayAlert(
                "Invalid Due Date",
                "The due date cannot be in the past.",
                "OK");

            return;
        }

        // -------------------------
        // CREATE TASK
        // -------------------------

        var task = new TaskItem
        {
            UserId = userId,

            Title = Title.Trim(),

            Description = Description.Trim(),

            Category = Category,

            Priority = Priority,

            DueDate = DueDate,

            IsCompleted = false
        };

        // -------------------------
        // SAVE TASK
        // -------------------------

        int result = await _database.AddTaskAsync(task);

        if (result <= 0)
        {
            await Shell.Current.DisplayAlert(
                "Error",
                "The task could not be saved. Please try again.",
                "OK");

            return;
        }

        await Shell.Current.DisplayAlert(
            "Task Added",
            "Your task has been added successfully.",
            "OK");

        await Shell.Current.GoToAsync("..");
    }
}