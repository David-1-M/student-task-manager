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
        await _database.CreateTableAsync<TaskItem>();
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

    public async Task<List<TaskItem>> GetTasksAsync()
    {
        return await Database.Table<TaskItem>()
            .ToListAsync();
    }

    public async Task<int> AddTaskAsync(TaskItem task)
    {
        return await Database.InsertAsync(task);
    }

    public async Task<int> UpdateTaskAsync(TaskItem task)
    {
        return await Database.UpdateAsync(task);
    }

    public async Task<TaskItem?> GetTaskByIdAsync(int id)
    {
        return await Database.Table<TaskItem>()
            .Where(t => t.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<int> DeleteTaskAsync(TaskItem task)
    {
        return await Database.DeleteAsync(task);
    }
}