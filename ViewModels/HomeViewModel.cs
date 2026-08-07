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

    [ObservableProperty]
    private string selectedSort = "Due Date";



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

        await RefreshTasks();

        TotalTasks = Tasks.Count;
        CompletedTasks = Tasks.Count(t => t.IsCompleted);
        PendingTasks = Tasks.Count(t => !t.IsCompleted);
        OnPropertyChanged(nameof(IsTaskListEmpty));

        OnPropertyChanged(nameof(TotalTasks));
        OnPropertyChanged(nameof(CompletedTasks));
        OnPropertyChanged(nameof(PendingTasks));
        OnPropertyChanged(nameof(IsTaskListEmpty));
    }

    partial void OnSearchTextChanged(string value)
    {
        _ = RefreshTasks();
        LoadTasksCommand.Execute(null);
    }

    partial void OnSelectedCategoryChanged(string value)
    {
        _ = RefreshTasks();
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

    public bool IsTaskListEmpty => Tasks.Count == 0;

    public List<string> SortOptions { get; } =
    new()
    {
        "Due Date",
        "Priority",
        "Title"
    };

    public List<string> FilterCategories { get; } =
    new()
    {
        "All",
        "School",
        "Work",
        "Personal",
        "Shopping",
        "Health",
        "Finance",
        "Other"
    };

    public int HighPriorityTasks =>
    Tasks.Count(t => t.Priority == "High");

    public int OverdueTasks =>
        Tasks.Count(t => t.IsOverdue);

    private async Task RefreshTasks()
    {
        Tasks.Clear();

        var allTasks = await _database.GetTasksAsync();

        IEnumerable<TaskItem> filtered = allTasks;

        if (SelectedCategory != "All")
        {
            filtered = filtered.Where(t =>
                t.Category == SelectedCategory);
        }

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            filtered = filtered.Where(t =>
                t.Title.Contains(SearchText,
                StringComparison.OrdinalIgnoreCase)
                ||
                t.Description.Contains(SearchText,
                StringComparison.OrdinalIgnoreCase));
        }

        foreach (var task in filtered)
        {
            Tasks.Add(task);
        }

        OnPropertyChanged(nameof(TotalTasks));
        OnPropertyChanged(nameof(CompletedTasks));
        OnPropertyChanged(nameof(PendingTasks));
        OnPropertyChanged(nameof(IsTaskListEmpty));
        OnPropertyChanged(nameof(HighPriorityTasks));
        OnPropertyChanged(nameof(OverdueTasks));
        OnPropertyChanged(nameof(CompletionProgress));
    }
    public double CompletionProgress =>
    TotalTasks == 0
        ? 0
        : (double)CompletedTasks / TotalTasks;

}