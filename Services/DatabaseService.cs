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

    public SQLiteAsyncConnection Database => _database!;

    public async Task<User> GetUserAsync(string email)
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
        return await _database.InsertAsync(user);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _database.Table<User>()
                              .Where(x => x.Email == email)
                              .FirstOrDefaultAsync();
    }

    public async Task<List<TaskItem>> GetTasksAsync()
    {
        return await Database.Table<TaskItem>().ToListAsync();
    }

    public async Task AddTaskAsync(TaskItem task)
    {
        await Database.InsertAsync(task);
    }
}