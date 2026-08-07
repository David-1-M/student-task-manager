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

    public ObservableCollection<TaskItem> Tasks { get; } = new();

    public HomeViewModel(DatabaseService database)
    {
        _database = database;
    }

    [ObservableProperty]
    private TaskItem? selectedTask;


    [ObservableProperty]
    private int totalTasks;

    [ObservableProperty]
    private int completedTasks;

    [ObservableProperty]
    private int pendingTasks;

    [ObservableProperty]
    private string searchText = "";

    [ObservableProperty]
    private string selectedCategory = "All";

    partial void OnSelectedCategoryChanged(string value)
    {
        FilterTasks();
    }

    partial void OnSearchTextChanged(string value)
    {
        FilterTasks();
    }

    if (SelectedCategory != "All")
    {
        filtered = filtered.Where(x =>
            x.Category == SelectedCategory);
    }

    private async void FilterTasks()
    {
        Tasks.Clear();

        var tasks = await _database.GetTasksAsync();

        var filtered = tasks.Where(t =>
            t.Title.Contains(SearchText ?? "",
            StringComparison.OrdinalIgnoreCase));

        foreach (var task in filtered)
            Tasks.Add(task);
    }

    public List<string> Categories { get; } =
    new()
    {
        "All",
        "Assignments",
        "Projects",
        "Tests",
        "Meetings",
        "Personal"
    };

    [RelayCommand]
    private async Task LoadTasks()
    {
        Tasks.Clear();

        var tasks = await _database.GetTasksAsync();

        foreach (var task in tasks)
            Tasks.Add(task);

        totalTasks = Tasks.Count;

        completedTasks = Tasks.Count(t => t.IsCompleted);

        pendingTasks = Tasks.Count(t => !t.IsCompleted);
    }

    [RelayCommand]
    private async Task AddTask()
    {
        await Shell.Current.GoToAsync(nameof(AddTaskPage));
    }

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