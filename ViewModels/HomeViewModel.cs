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

    public List<string> SortOptions { get; } =
    [
        "Due Date",
        "Priority",
        "Title"
    ];

    [ObservableProperty]
    private TaskItem? selectedTask;

    [ObservableProperty]
    private string searchText = string.Empty;

    [ObservableProperty]
    private string selectedCategory = "All";

    [ObservableProperty]
    private string selectedSort = "Due Date";

    [ObservableProperty]
    private int totalTasks;

    [ObservableProperty]
    private int completedTasks;

    [ObservableProperty]
    private int pendingTasks;

    [ObservableProperty]
    private int overdueTasks;

    [ObservableProperty]
    private int highPriorityTasks;

    public bool IsTaskListEmpty => Tasks.Count == 0;

    public double CompletionProgress =>
        TotalTasks == 0
            ? 0
            : (double)CompletedTasks / TotalTasks;

    public HomeViewModel(DatabaseService database)
    {
        _database = database;
    }

    // -------------------------
    // LOAD TASKS
    // -------------------------

    [RelayCommand]
    private async Task LoadTasks()
    {
        await RefreshTasks();
    }

    // -------------------------
    // REFRESH / FILTER / SORT
    // -------------------------

    private async Task RefreshTasks()
    {
        var allTasks = await _database.GetTasksAsync();

        // Dashboard statistics use ALL tasks,
        // not just the currently filtered list.
        TotalTasks = allTasks.Count;

        CompletedTasks =
            allTasks.Count(t => t.IsCompleted);

        PendingTasks =
            allTasks.Count(t => !t.IsCompleted);

        OverdueTasks =
            allTasks.Count(t => t.IsOverdue);

        HighPriorityTasks =
            allTasks.Count(t =>
                t.Priority == "High" && !t.IsCompleted);

        IEnumerable<TaskItem> filtered = allTasks;

        // Search
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            filtered = filtered.Where(t =>
                t.Title.Contains(
                    SearchText,
                    StringComparison.OrdinalIgnoreCase)
                ||
                t.Description.Contains(
                    SearchText,
                    StringComparison.OrdinalIgnoreCase)
                ||
                t.Category.Contains(
                    SearchText,
                    StringComparison.OrdinalIgnoreCase));
        }

        // Category
        if (SelectedCategory != "All")
        {
            filtered = filtered.Where(t =>
                t.Category == SelectedCategory);
        }

        // Sorting
        filtered = SelectedSort switch
        {
            "Priority" => filtered
                .OrderBy(t => GetPriorityValue(t.Priority))
                .ThenBy(t => t.DueDate),

            "Title" => filtered
                .OrderBy(t => t.Title),

            _ => filtered
                .OrderBy(t => t.IsCompleted)
                .ThenBy(t => t.DueDate)
        };

        Tasks.Clear();

        foreach (var task in filtered)
        {
            Tasks.Add(task);
        }

        OnPropertyChanged(nameof(IsTaskListEmpty));
        OnPropertyChanged(nameof(CompletionProgress));
    }

    private static int GetPriorityValue(string priority)
    {
        return priority switch
        {
            "High" => 0,
            "Medium" => 1,
            "Low" => 2,
            _ => 3
        };
    }

    // -------------------------
    // SEARCH
    // -------------------------

    partial void OnSearchTextChanged(string value)
    {
        _ = RefreshTasks();
    }

    // -------------------------
    // CATEGORY FILTER
    // -------------------------

    partial void OnSelectedCategoryChanged(string value)
    {
        _ = RefreshTasks();
    }

    // -------------------------
    // SORTING
    // -------------------------

    partial void OnSelectedSortChanged(string value)
    {
        _ = RefreshTasks();
    }

    // -------------------------
    // SELECT TASK
    // -------------------------

    partial void OnSelectedTaskChanged(TaskItem? value)
    {
        if (value == null)
            return;

        _ = OpenTask(value);

        SelectedTask = null;
    }

    private async Task OpenTask(TaskItem task)
    {
        await Shell.Current.GoToAsync(
            $"{nameof(EditTaskPage)}?TaskId={task.Id}");
    }

    // -------------------------
    // ADD TASK
    // -------------------------

    [RelayCommand]
    private async Task AddTask()
    {
        await Shell.Current.GoToAsync(
            nameof(AddTaskPage));
    }

    // -------------------------
    // COMPLETE TASK
    // -------------------------

    [RelayCommand]
    private async Task CompleteTask(TaskItem task)
    {
        if (task == null)
            return;

        if (task.IsCompleted)
            return;

        task.IsCompleted = true;

        await _database.UpdateTaskAsync(task);

        await RefreshTasks();
    }

    // -------------------------
    // DELETE TASK
    // -------------------------

    [RelayCommand]
    private async Task DeleteTask(TaskItem task)
    {
        if (task == null)
            return;

        bool answer = await Shell.Current.DisplayAlert(
            "Delete Task",
            $"Are you sure you want to delete '{task.Title}'?",
            "Yes",
            "No");

        if (!answer)
            return;

        await _database.DeleteTaskAsync(task);

        await RefreshTasks();
    }
}