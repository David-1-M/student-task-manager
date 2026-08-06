using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.VisualBasic;
using StudentTaskManager.Models;
using StudentTaskManager.Services;


namespace StudentTaskManager.ViewModels;

public partial class EditTaskViewModel : ObservableObject
{
    private readonly DatabaseService _database;

    private TaskItem? _task;

    [ObservableProperty]
    private string title = "";

    [ObservableProperty]
    private string description = "";

    [ObservableProperty]
    private string category = "";

    [ObservableProperty]
    private DateTime dueDate = DateTime.Today;

    public EditTaskViewModel(DatabaseService database)
    {
        _database = database;
    }

    [RelayCommand]
    private async Task LoadTask(int id)
    {
        _task = await _database.GetTaskByIdAsync(id);

        if (_task == null)
            return;

        Title = _task.Title;
        Description = _task.Description;
        Category = _task.Category;
        DueDate = _task.DueDate;
    }

    [RelayCommand]
    private async Task Save()
    {
        if (_task == null)
            return;

        _task.Title = Title;
        _task.Description = Description;
        _task.Category = Category;
        _task.DueDate = DueDate;

        await _database.UpdateTaskAsync(_task);

        await Shell.Current.GoToAsync("..");
    }
}