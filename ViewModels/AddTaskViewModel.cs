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
    private DateTime dueDate = DateTime.Today;

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
            IsCompleted = false
        };

        await _database.AddTaskAsync(task);

        await Shell.Current.GoToAsync("..");
    }
}