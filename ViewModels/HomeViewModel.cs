using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudentTaskManager.Models;
using StudentTaskManager.Services;
using StudentTaskManager.Views;
using System.Collections.ObjectModel;

namespace StudentTaskManager.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    private readonly DatabaseService _database;

    public ObservableCollection<TaskItem> Tasks { get; } = new();

    public List<string> Categories { get; } =
    [
        "All",
        "Assignments",
        "Projects",
        "Tests",
        "Meetings",
        "Personal",
        "Other"
    ];

    [ObservableProperty]
    private TaskItem? selectedTask;

    [ObservableProperty]
    private string searchText = "";

    [ObservableProperty]
    private string selectedCategory = "All";

    [ObservableProperty]
    private int totalTasks;

    [ObservableProperty]
    private int completedTasks;

    [ObservableProperty]
    private int pendingTasks;

    public HomeViewModel(DatabaseService database)
    {
        _database = database;
    }

    [RelayCommand]
    private async Task LoadTasks()
    {
        Tasks.Clear();

        var tasks = await _database.GetTasksAsync();

        IEnumerable<TaskItem> filtered = tasks;

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            filtered = filtered.Where(t =>
                t.Title.Contains(SearchText,
                StringComparison.OrdinalIgnoreCase));
        }

        if (SelectedCategory != "All")
        {
            filtered = filtered.Where(t =>
                t.Category == SelectedCategory);
        }

        foreach (var task in filtered)
        {
            Tasks.Add(task);
        }

        TotalTasks = Tasks.Count;
        CompletedTasks = Tasks.Count(t => t.IsCompleted);
        PendingTasks = Tasks.Count(t => !t.IsCompleted);
    }

    partial void OnSearchTextChanged(string value)
    {
        LoadTasksCommand.Execute(null);
    }

    partial void OnSelectedCategoryChanged(string value)
    {
        LoadTasksCommand.Execute(null);
    }

    partial void OnSelectedTaskChanged(TaskItem? value)
    {
        if (value == null)
            return;

        Shell.Current.GoToAsync($"{nameof(EditTaskPage)}?TaskId={value.Id}");

        SelectedTask = null;
    }

    [RelayCommand]
    private async Task AddTask()
    {
        await Shell.Current.GoToAsync(nameof(AddTaskPage));
    }
}