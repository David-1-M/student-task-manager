using StudentTaskManager.Models;

namespace StudentTaskManager.Services;

public class AuthenticationService
{
    private readonly DatabaseService _database;

    public AuthenticationService(DatabaseService database)
    {
        _database = database;
    }

    public async Task<bool> Register(User user)
    {
        var existing =
            await _database.GetUserByEmailAsync(user.Email);

        if (existing != null)
            return false;

        await _database.AddUserAsync(user);

        return true;
    }

    public async Task<User?> Login(string email, string password)
    {
        var user =
            await _database.GetUserByEmailAsync(email);

        if (user == null)
            return null;

        if (user.Password != password)
            return null;

        return user;
    }
}