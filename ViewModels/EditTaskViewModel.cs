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
    private string category = "School";

    [ObservableProperty]
    private string priority = "Medium";

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

    [RelayCommand]
    private async Task LoadTask(int id)
    {
        _task = await _database.GetTaskByIdAsync(id);

        if (_task == null)
            return;

        Title = _task.Title;
        Description = _task.Description;
        Category = _task.Category;
        Priority = _task.Priority;
        DueDate = _task.DueDate;
        IsCompleted = _task.IsCompleted;
    }

    [RelayCommand]
    private async Task Save()
    {
        if (_task == null)
            return;

        if (string.IsNullOrWhiteSpace(Title))
        {
            await Shell.Current.DisplayAlert(
                "Error",
                "Task title is required.",
                "OK");

            return;
        }

        _task.Title = Title.Trim();
        _task.Description = Description.Trim();
        _task.Category = Category;
        _task.Priority = Priority;
        _task.DueDate = DueDate;
        _task.IsCompleted = IsCompleted;

        await _database.UpdateTaskAsync(_task);

        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task Delete()
    {
        if (_task == null)
            return;

        bool answer = await Shell.Current.DisplayAlert(
            "Delete Task",
            $"Are you sure you want to delete '{_task.Title}'?",
            "Yes",
            "No");

        if (!answer)
            return;

        await _database.DeleteTaskAsync(_task);

        await Shell.Current.GoToAsync("..");
    }
}