using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagamentSystem.Api.DTOs;
using TaskManagamentSystem.Api.Models.Enums;
using TaskManagamentSystem.Api.Services.Interfaces;
using TaskManagamentSystem.Api.Helpers;

namespace TaskManagamentSystem.Api.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TasksController> _logger;

        public TasksController(ITaskService taskService, ILogger<TasksController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }


        /// <summary>
        ///  Returns all tasks
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetTasks([FromQuery] Models.Enums.TaskStatus? status, [FromQuery] Guid? assigneeId)
        {
            _logger.LogInformation("Fetching all the created tasks");
            var response = await _taskService.GetTasks(status, assigneeId);

            return StatusCode(response.StatusCode, response);

        }

        /// <summary
        /// Creates a new task assigned to the current user
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskCreateDto dto)
        {
            var userId = User.GetUserId();
            var createdTask = await _taskService.CreateTask(userId, dto);

            return StatusCode(createdTask.StatusCode, createdTask);
        }

        /// <summary>
        /// Updates an existing Task using Guid
        /// </summary>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateTask([FromRoute] Guid id, [FromBody] UpdateTaskDto dto)
        {
            var updatedTask = await _taskService.UpdateTask(id, dto);

            return StatusCode(updatedTask.StatusCode, updatedTask);
            
        }

        /// <summary>
        /// Delete Existing Task : ROLE: Admin 
        /// </summary>
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "USER")]
        public async Task<IActionResult> DeleteTask([FromRoute] Guid id)
        {
            var deletedTask = await _taskService.DeleteTask(id);

            //return
            return StatusCode(deletedTask.StatusCode, deletedTask);
        }
    }
}