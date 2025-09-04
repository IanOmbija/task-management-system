using Microsoft.EntityFrameworkCore;
using TaskManagamentSystem.Api.Data;
using TaskManagamentSystem.Api.DTOs;
using TaskManagamentSystem.Api.Models;
using TaskManagamentSystem.Api.Models.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;

namespace TaskManagamentSystem.Api.Services;

public class TaskService : Services.Interfaces.ITaskService
{
    private ResponseDto _responseDto = new();
    private readonly ApplicationDbContext _dbContext;
    public TaskService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ResponseDto> GetTasks(Models.Enums.TaskStatus? status, Guid? assigneeId)
    {
        try
        {
            var queryTasks = _dbContext.Tasks.AsNoTracking().AsQueryable();

            if (status.HasValue) queryTasks = queryTasks.Where(x => x.Status == status);
            if (assigneeId.HasValue) queryTasks = queryTasks.Where(x => x.AssigneeId == assigneeId);

            var tasks = await queryTasks.OrderByDescending(x => x.UpdatedAt)
                .Select(x => new TaskResponse
                {
                    Id = x.Id,
                    Tittle = x.Tittle,
                    Description = x.Description,
                    Status = x.Status.ToString(),
                    TaskPriority = x.taskPriority.ToString(),
                    AssigneeId = x.AssigneeId,
                    CreatorId = x.CreatorId,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .ToListAsync();

            if (tasks.Any())
            {
                _responseDto.StatusCode = StatusCodes.Status200OK;
                _responseDto.Message = $"Found {tasks.Count} task(s).";
                _responseDto.Payload = new
                {
                    Output = tasks,
                    RowCount = tasks.Count
                };
            }
            else
            {
                _responseDto.StatusCode = StatusCodes.Status404NotFound;
                _responseDto.Message = "No tasks found. Please create one to view.";
            }
        }
        catch (Exception ex)
        {
            _responseDto.StatusCode = StatusCodes.Status500InternalServerError;
            _responseDto.Message = "Something went wrong. Unable to fetch tasks.";
            _responseDto.Payload = new
            {
                ex.Message,
                ex.StackTrace,
                ex.InnerException,
                ex.Source
            };
        }

        return _responseDto;
    }

    public async Task<ResponseDto> CreateTask(Guid creatorId, TaskCreateDto dto)
    {
        try
        {
            var newTask = new Models.Task
            {
                Tittle = dto.Tittle,
                Description = dto.Description,
                taskPriority = dto.taskPriority,
                Status = dto.taskStatus,
                AssigneeId = dto.AssigneeId,
                CreatorId = creatorId
            };

            _dbContext.Tasks.Add(newTask);
            await _dbContext.SaveChangesAsync();

            var result = new TaskResponse
            {
                Id = newTask.Id,
                Tittle = newTask.Tittle,
                Description = newTask.Description,
                Status = newTask.Status.ToString(),
                TaskPriority = newTask.taskPriority.ToString(),
                AssigneeId = newTask.AssigneeId,
                CreatorId = newTask.CreatorId,
                CreatedAt = newTask.CreatedAt,
                UpdatedAt = newTask.UpdatedAt
            };

            _responseDto.StatusCode = StatusCodes.Status201Created;
            _responseDto.Message = "Task created successfully.";
            _responseDto.Payload = result;
        }
        catch (Exception ex)
        {
            _responseDto.StatusCode = StatusCodes.Status500InternalServerError;
            _responseDto.Message = "Unable to create task. Something went wrong.";
            _responseDto.Payload = new
            {
                ex.Message,
                ex.StackTrace,
                ex.InnerException,
                ex.Source
            };
        }

        return _responseDto;
    }

    public async Task<ResponseDto> UpdateTask(Guid taskId, UpdateTaskDto dto)
    {
        var _responseDto = new ResponseDto();
        try
        {
            var task = await _dbContext.Tasks.FirstOrDefaultAsync(existingTask => existingTask.Id == taskId);
            //var task = await _dbContext.Tasks.FindAsync(taskId);
            if (task == null)
            {
                _responseDto.Message = $"Invalid Task ID: {taskId}: Please provide a valid task ID";
                _responseDto.StatusCode = StatusCodes.Status404NotFound;

                //return
                return _responseDto;
            }

            //Update
            task.Tittle = dto.Tittle ?? task.Tittle;
            task.Description = dto.Description ?? task.Description;
            task.Status = dto.status;
            task.taskPriority = dto.priority;
            task.AssigneeId = dto.AssigneeId ?? task.AssigneeId;
            task.UpdatedAt = DateTime.UtcNow;

            _dbContext.Tasks.Update(task);
            await _dbContext.SaveChangesAsync();

            _responseDto.Message = "Task updated successfully.";
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Payload = task;

        }
        catch (Exception ex)
        {
            _responseDto.StatusCode = StatusCodes.Status500InternalServerError;
            _responseDto.Message = "Task update failed: Something went wrong.";
            _responseDto.Payload = new
            {
                ex.Message,
                ex.StackTrace,
                ex.InnerException,
                ex.Source
            };
        }

        return _responseDto;
    }

    public async Task<ResponseDto> DeleteTask(Guid taskId)
    {
        var _responseDto = new ResponseDto();

        try
        {
            var taskToDelete = await _dbContext.Tasks.FindAsync(taskId);
            if (taskToDelete == null)
            {
                _responseDto.Message = "Invalid Task ID. Please provide a valid Task ID";
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

                //return
                return _responseDto;
            }

            _dbContext.Tasks.Remove(taskToDelete);
            await _dbContext.SaveChangesAsync();

            _responseDto.StatusCode = StatusCodes.Status204NoContent;
            _responseDto.Message = "Task deleted successfully.";
        }
        catch (Exception ex)
        {
            _responseDto.StatusCode = StatusCodes.Status500InternalServerError;
            _responseDto.Message = "Task deletion failed: Something went wrong.";
            _responseDto.Payload = new
            {
                ex.Message,
                ex.StackTrace,
                ex.InnerException,
                ex.Source
            };
        }

        return _responseDto;
    }

}