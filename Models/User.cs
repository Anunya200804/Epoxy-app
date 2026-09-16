namespace EpoxyFloorManager.Models;

public enum UserRole
{
    Admin,
    ProjectManager,
    Staff
}

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Phone { get; set; }
}
