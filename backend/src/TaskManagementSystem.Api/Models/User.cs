using TaskManagamentSystem.Api.Models.Enums;

namespace TaskManagamentSystem.Api.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public Role Role { get; set; } = Role.USER;
    public DateTime createdAt { get; set; } = DateTime.UtcNow;

    public ICollection<Task> CreatedTasks { get; set; } = new List<Task>();
    public ICollection<Task> AssignedTasks { get; set; } = new List<Task>();

}