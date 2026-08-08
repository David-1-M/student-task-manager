using SQLite;
using StudentTaskManager.Models;

namespace StudentTaskManager.Services;

public class DatabaseService
{
    private SQLiteAsyncConnection? _database;

    public async Task InitializeAsync()
    {
        if (_database != null)
            return;

        string dbPath = Path.Combine(
            FileSystem.AppDataDirectory,
            "StudentTaskManager.db");

        _database = new SQLiteAsyncConnection(dbPath);

        await _database.CreateTableAsync<User>();

        // Create the TaskItem table if it doesn't exist.
        await _database.CreateTableAsync<TaskItem>();

        // Add UserId to existing databases created before
        // multi-user support was introduced.
        await MigrateTaskTableAsync();
    }

    private async Task MigrateTaskTableAsync()
    {
        var columns = await Database.GetTableInfoAsync("TaskItem");

        bool hasUserId = columns.Any(
            column => column.Name == nameof(TaskItem.UserId));

        if (!hasUserId)
        {
            await Database.ExecuteAsync(
                "ALTER TABLE TaskItem ADD COLUMN UserId INTEGER NOT NULL DEFAULT 0");
        }
    }

    public SQLiteAsyncConnection Database =>
        _database ?? throw new InvalidOperationException(
            "Database has not been initialized.");

    // -------------------------
    // USER OPERATIONS
    // -------------------------

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await Database.Table<User>()
            .Where(x => x.Email == email)
            .FirstOrDefaultAsync();
    }

    public async Task<List<User>> GetUsersAsync()
    {
        return await Database.Table<User>().ToListAsync();
    }

    public async Task<int> AddUserAsync(User user)
    {
        return await Database.InsertAsync(user);
    }

    // -------------------------
    // TASK OPERATIONS
    // -------------------------

    public async Task<List<TaskItem>> GetTasksAsync(int userId)
    {
        return await Database.Table<TaskItem>()
            .Where(t => t.UserId == userId)
            .ToListAsync();
    }

    public async Task<int> AddTaskAsync(TaskItem task)
    {
        return await Database.InsertAsync(task);
    }

    public async Task<int> UpdateTaskAsync(TaskItem task)
    {
        var existingTask = await Database.Table<TaskItem>()
            .Where(t =>
                t.Id == task.Id &&
                t.UserId == task.UserId)
            .FirstOrDefaultAsync();

        if (existingTask == null)
            return 0;

        return await Database.UpdateAsync(task);
    }

    public async Task<TaskItem?> GetTaskByIdAsync(
        int id,
        int userId)
    {
        return await Database.Table<TaskItem>()
            .Where(t =>
                t.Id == id &&
                t.UserId == userId)
            .FirstOrDefaultAsync();
    }

    public async Task<int> DeleteTaskAsync(TaskItem task)
    {
        var existingTask = await Database.Table<TaskItem>()
            .Where(t =>
                t.Id == task.Id &&
                t.UserId == task.UserId)
            .FirstOrDefaultAsync();

        if (existingTask == null)
            return 0;

        return await Database.DeleteAsync(task);
    }

    // -------------------------
    // LEGACY TASKS
    // -------------------------

    public async Task<int> AssignLegacyTasksToUserAsync(int userId)
    {
        return await Database.ExecuteAsync(
            "UPDATE TaskItem SET UserId = ? WHERE UserId = 0",
            userId);
    }
}