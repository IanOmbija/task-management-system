using System.ComponentModel.DataAnnotations;
using TaskManagamentSystem.Api.Models.Enums;
using TaskStatus = TaskManagamentSystem.Api.Models.Enums.TaskStatus;

namespace TaskManagamentSystem.Api.DTOs;

public class TaskCreateDto
{
    [Required, MinLength(3)] public string Tittle { get; set; } = default!;
    public string? Description { get; set; }
    public TaskPriority taskPriority { get; set; } = TaskPriority.MEDIUM;

    public TaskStatus taskStatus { get; set; } = TaskStatus.TODO;
    public Guid? AssigneeId { get; set; }
}

public class UpdateTaskDto
{
    [Required, MinLength(3)] public string Tittle { get; set; } = default!;
    public string? Description { get; set; }
    public TaskPriority priority { get; set; }
    public TaskStatus status { get; set; }
    public Guid? AssigneeId { get; set; }
}

public class TaskResponse
{
    public Guid Id { get; set; }
    public string Tittle { get; set; }
    public string? Description { get; set; }
    public string Status { get; set; } = default!;
    public string TaskPriority { get; set; } = default!;
    public Guid? AssigneeId { get; set; }
    public Guid CreatorId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}