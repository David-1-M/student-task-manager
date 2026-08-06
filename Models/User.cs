using SQLite;

namespace StudentTaskManager.Models;

public class User
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    [Unique]
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}