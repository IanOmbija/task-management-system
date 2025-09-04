using TaskManagamentSystem.Api.Models.Enums;

namespace TaskManagamentSystem.Api.Models;

public class Task
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Tittle { get; set; } = default!;
    public string? Description { get; set; }
    public Enums.TaskStatus Status { get; set; } = Enums.TaskStatus.TODO;
    public TaskPriority taskPriority { get; set; } = TaskPriority.MEDIUM;

    public Guid? AssigneeId { get; set; }
    public User? Assignee { get; set; }

    public Guid CreatorId { get; set; }
    public User Creator { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    

}