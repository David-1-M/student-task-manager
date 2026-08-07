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
    private string priority = "Medium";

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
        if (string.IsNullOrWhiteSpace(Title))
        {
            await Shell.Current.DisplayAlert(
                "Missing Title",
                "Please enter a task title.",
                "OK");

            return;
        }

        if (string.IsNullOrWhiteSpace(Category))
        {
            Category = "Other";
        }

        var task = new TaskItem
        {
            Title = Title.Trim(),
            Description = Description?.Trim() ?? string.Empty,
            Category = Category,
            Priority = Priority,
            DueDate = DueDate,
            IsCompleted = false
        };

        await _database.AddTaskAsync(task);

        await Shell.Current.DisplayAlert(
            "Task Added",
            "Your task has been added successfully.",
            "OK");

        await Shell.Current.GoToAsync("..");
    }
}