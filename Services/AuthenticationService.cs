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
        await _database.InitializeAsync();

        string email = user.Email.Trim().ToLowerInvariant();

        var existingUser =
            await _database.GetUserByEmailAsync(email);

        if (existingUser != null)
            return false;

        user.Email = email;

        // Never store the user's password directly.
        user.Password = BCrypt.Net.BCrypt.HashPassword(
            user.Password);

        await _database.AddUserAsync(user);

        return true;
    }

    public async Task<User?> Login(
        string email,
        string password)
    {
        await _database.InitializeAsync();

        string normalizedEmail =
            email.Trim().ToLowerInvariant();

        var user =
            await _database.GetUserByEmailAsync(
                normalizedEmail);

        if (user == null)
            return null;

        bool passwordCorrect =
            BCrypt.Net.BCrypt.Verify(
                password,
                user.Password);

        if (!passwordCorrect)
            return null;

        return user;
    }
}