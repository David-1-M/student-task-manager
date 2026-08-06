using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudentTaskManager.Models;
using StudentTaskManager.Services;
using System.Collections.ObjectModel;
using StudentTaskManager.Views;

namespace StudentTaskManager.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    private readonly DatabaseService _database;

    public ObservableCollection<TaskItem> Tasks { get; }
        = new();

    public HomeViewModel(DatabaseService database)
    {
        _database = database;
    }

    [RelayCommand]
    private async Task LoadTasks()
    {
        Tasks.Clear();

        var tasks = await _database.GetTasksAsync();

        foreach (var task in tasks)
            Tasks.Add(task);
    }

    [RelayCommand]
    private async Task AddTask()
    {
        await Shell.Current.GoToAsync(nameof(AddTaskPage));
    }

    [ObservableProperty]
    private TaskItem? selectedTask;

    partial void OnSelectedTaskChanged(TaskItem? value)
    {
        if (value == null)
            return;

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await Shell.Current.GoToAsync(
                $"{nameof(EditTaskPage)}?TaskId={value.Id}");

            SelectedTask = null;
        });
    }
}