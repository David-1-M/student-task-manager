using BCrypt.Net;
using StudentTaskManager.Models;

namespace StudentTaskManager.Services;

public class AuthenticationService
{
    private readonly DatabaseService _database;

    public AuthenticationService(DatabaseService database)
    {
        _database = database;
    }

    public async Task<bool> RegisterAsync(string name, string email, string password)
    {
        await _database.InitializeAsync();

        var existingUser = await _database.Database.Table<User>()
            .Where(u => u.Email == email)
            .FirstOrDefaultAsync();

        if (existingUser != null)
            return false;

        var user = new User
        {
            Name = name,
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
        };

        await _database.Database.InsertAsync(user);

        return true;
    }

    public async Task<User?> LoginAsync(string email, string password)
    {
        await _database.InitializeAsync();

        var user = await _database.Database.Table<User>()
            .Where(u => u.Email == email)
            .FirstOrDefaultAsync();

        if (user == null)
            return null;

        return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)
            ? user
            : null;
    }
}