using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudentTaskManager.Models;
using StudentTaskManager.Services;

namespace StudentTaskManager.ViewModels;

public partial class EditTaskViewModel : ObservableObject
{
    private readonly DatabaseService _database;

    private TaskItem? _task;

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
    private bool isCompleted;

    public EditTaskViewModel(DatabaseService database)
    {
        _database = database;
    }

    public List<string> Categories { get; } =
    [
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
    ];

    public List<string> Priorities { get; } =
    [
        "High",
        "Medium",
        "Low"
    ];

    // -------------------------
    // CURRENT USER
    // -------------------------

    private int GetCurrentUserId()
    {
        return Preferences.Default.Get(
            "LoggedInUserId",
            0);
    }

    // -------------------------
    // LOAD TASK
    // -------------------------

    [RelayCommand]
    private async Task LoadTask(int id)
    {
        int userId = GetCurrentUserId();

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

        // Only load a task belonging to the logged-in user.
        _task = await _database.GetTaskByIdAsync(
            id,
            userId);

        if (_task == null)
        {
            await Shell.Current.DisplayAlert(
                "Task Not Found",
                "This task could not be found or does not belong to your account.",
                "OK");

            await Shell.Current.GoToAsync("..");

            return;
        }

        Title = _task.Title;
        Description = _task.Description;
        Category = _task.Category;
        Priority = _task.Priority;
        DueDate = _task.DueDate;
        IsCompleted = _task.IsCompleted;
    }

    // -------------------------
    // SAVE
    // -------------------------

    [RelayCommand]
    private async Task Save()
    {
        if (_task == null)
            return;

        int userId = GetCurrentUserId();

        if (userId == 0)
        {
            await Shell.Current.DisplayAlert(
                "Not Logged In",
                "Your session has expired. Please log in again.",
                "OK");

            return;
        }

        // Security check
        if (_task.UserId != userId)
        {
            await Shell.Current.DisplayAlert(
                "Access Denied",
                "You cannot edit this task.",
                "OK");

            return;
        }

        // -------------------------
        // VALIDATE TITLE
        // -------------------------

        if (string.IsNullOrWhiteSpace(Title))
        {
            await Shell.Current.DisplayAlert(
                "Missing Title",
                "Task title is required.",
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
                "Task description is required.",
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
        // UPDATE TASK
        // -------------------------

        _task.Title = Title.Trim();
        _task.Description = Description.Trim();
        _task.Category = Category;
        _task.Priority = Priority;
        _task.DueDate = DueDate;
        _task.IsCompleted = IsCompleted;

        int result = await _database.UpdateTaskAsync(_task);

        if (result <= 0)
        {
            await Shell.Current.DisplayAlert(
                "Error",
                "The task could not be updated.",
                "OK");

            return;
        }

        await Shell.Current.GoToAsync("..");
    }

    // -------------------------
    // DELETE
    // -------------------------

    [RelayCommand]
    private async Task Delete()
    {
        if (_task == null)
            return;

        int userId = GetCurrentUserId();

        if (userId == 0)
            return;

        // Security check
        if (_task.UserId != userId)
        {
            await Shell.Current.DisplayAlert(
                "Access Denied",
                "You cannot delete this task.",
                "OK");

            return;
        }

        bool answer = await Shell.Current.DisplayAlert(
            "Delete Task",
            $"Are you sure you want to delete '{_task.Title}'?",
            "Yes",
            "No");

        if (!answer)
            return;

        int result = await _database.DeleteTaskAsync(_task);

        if (result <= 0)
        {
            await Shell.Current.DisplayAlert(
                "Error",
                "The task could not be deleted.",
                "OK");

            return;
        }

        await Shell.Current.GoToAsync("..");
    }
}