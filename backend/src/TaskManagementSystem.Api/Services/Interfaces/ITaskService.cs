using TaskManagamentSystem.Api.DTOs;
using TaskManagamentSystem.Api.Models.Enums;

namespace TaskManagamentSystem.Api.Services.Interfaces;

public interface ITaskService
{
    Task<ResponseDto> GetTasks(Models.Enums.TaskStatus? status, Guid? assigneeId);
    Task<ResponseDto> CreateTask(Guid creatorId, TaskCreateDto dto);
    Task<ResponseDto> UpdateTask(Guid id, UpdateTaskDto dto);
    Task<ResponseDto> DeleteTask(Guid id);
}