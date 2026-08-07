using SQLite;

namespace StudentTaskManager.Models;

public class TaskItem
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string Priority { get; set; } = "Medium";

    public DateTime DueDate { get; set; }

    public bool IsCompleted { get; set; }


    public string StatusText =>
    IsCompleted ? "✅ Completed" : "⏳ Pending";

    public string PriorityDisplay =>
    Priority switch
    {
        "High" => "🔴 High",
        "Medium" => "🟡 Medium",
        _ => "🟢 Low"
    };

    public bool IsOverdue =>
    !IsCompleted &&
    DueDate.Date < DateTime.Today;

    public string DueStatus
    {
        get
        {
            if (IsCompleted)
                return "✅ Completed";

            if (IsOverdue)
                return "⚠ Overdue";

            return "📅 Upcoming";
        }
    }

    public Color PriorityColor
    {
        get
        {
            return Priority switch
            {
                "High" => Colors.Red,
                "Medium" => Colors.Orange,
                _ => Colors.Green
            };
        }
    }

}