using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManagamentSystem.Api.Models;
using TaskManagamentSystem.Api.DTOs;
using TasksEnums = TaskManagamentSystem.Api.Models.Enums;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Net.Mime;
using TaskManagamentSystem.Api.Data;
using TaskEntity = TaskManagamentSystem.Api.Models.Task;
using TaskManagamentSystem.Api.Models.Enums;

public class TaskControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public TaskControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();

        var testUserId = Guid.NewGuid().ToString();

        var token = AuthHelper.GenerateToken("ZxKqYqv+2l7z9FN9u8dJngQK4iUbZfKzjQp6C0p5W4Q=", testUserId);
        Console.WriteLine("JWT Token Generated:\n " + token);
        Console.WriteLine("Test User ID Generated: " + testUserId);
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetTasks_ShouldReturnEmpty_WhenNoTasksExist()
    {
        var response = await _client.GetAsync("/api/tasks");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var readJson = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Received Data:\n " + readJson);

        var result = await response.Content.ReadFromJsonAsync<ResponseDto>();

        // Validations
        result.Should().NotBeNull();
        result!.Message.Should().Be("No tasks found. Please create one to view.");
        result.Payload.Should().BeNull(); 

    }

    [Fact]
    public async System.Threading.Tasks.Task CreateTask_ShouldReturnCreatedTask()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        //var creatorId = Guid.NewGuid();
        var validUser = new User
        {
            Id = Guid.NewGuid(),
            Username = "Test Usr",
            Email = "test@test.com",
            PasswordHash = "pasword_hashed"
        };
        context.Users.Add(validUser);
        await context.SaveChangesAsync();

        var newTask = new TaskEntity
        {
            Tittle = "Test Task",
            Description = "Test Desc",
            Status = TasksEnums.TaskStatus.DONE,
            taskPriority = TaskPriority.LOW,
            AssigneeId = validUser.Id,
            //CreatorId = creatorId
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/tasks", newTask);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var responseJson = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Received JSON Response :\n" + responseJson);

        var createdTask = await response.Content.ReadFromJsonAsync<JsonElement>();
        createdTask.Should().NotBeNull();
        //createdTask.GetProperty("tittle").GetString().Should().Be("Test Task");
        //createdTask.AssigneeId.Should().Be(validUser.Id);
        var payload = createdTask.GetProperty("payload");
        payload.GetProperty("tittle").GetString().Should().Be("Test Task");
        payload.GetProperty("assigneeId").GetString().Should().Be(validUser.Id.ToString());
    }

    [Fact]
    public async System.Threading.Tasks.Task UpdateTask_ShouldReturnUpdatedTask()
    {
        //
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var existingTask = new TaskEntity
        {
            Tittle = "Old Task",
            Description = "Old Desc",
            taskPriority = TasksEnums.TaskPriority.LOW,
            Status = TasksEnums.TaskStatus.TODO,
            AssigneeId = Guid.NewGuid()
        };

        context.Tasks.Add(existingTask);
        await context.SaveChangesAsync();

        var updatedTask = new
        {
            existingTask.Id,
            Tittle = "Updated Task",
            Description = "Updated Desc"
        };

        var response = await _client.PutAsJsonAsync($"/api/tasks/{existingTask.Id}", updatedTask);
        var result = await response.Content.ReadFromJsonAsync<JsonElement>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();

        var payload = result.GetProperty("payload");
        payload.GetProperty("tittle").GetString().Should().Be("Updated Task");
        payload.GetProperty("description").GetString().Should().Be("Updated Desc");
      
    }

    [Fact]
    public async System.Threading.Tasks.Task DeleteTask_ShouldReturnNoContent_WhenTaskIsDeleted()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var newTask = new TaskEntity
        {
            Tittle = "Test Task",
            Description = "Test Description",
            AssigneeId = Guid.NewGuid()
        };

        context.Tasks.Add(newTask);
        await context.SaveChangesAsync();

        // Act
        var response = await _client.DeleteAsync($"/api/tasks/{newTask.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        context.Entry(newTask).State = EntityState.Detached;

        var deletedTask = await context.Tasks.FindAsync(newTask.Id);
        deletedTask.Should().BeNull();
    }

    [Fact]
    public async System.Threading.Tasks.Task GetTasks_FilterByStatus_ShouldReturnOnlyMatching()
    {
        //Arrange
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var completedTask = new TaskEntity
        {
            Tittle = "Task TODO Complete",
            Description = "TODO Task Status",
            taskPriority = TasksEnums.TaskPriority.MEDIUM,
            Status = TasksEnums.TaskStatus.DONE,
            AssigneeId = Guid.Parse("11111111-1111-1111-1111-111111111111")
        };

        var inProgressTask = new TaskEntity
        {
            Tittle = "Task IN_PROGRESS",
            Description = " Task in progress",
            taskPriority = TasksEnums.TaskPriority.MEDIUM,
            Status = TasksEnums.TaskStatus.IN_PROGRESS,
            AssigneeId = Guid.Parse("11111111-1111-1111-1111-111111111111")
        };

        context.Tasks.AddRange(completedTask, inProgressTask);
        await context.SaveChangesAsync();

        var response = await _client.GetAsync("/api/tasks?status=DONE");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        result.ToString().Should().Contain("DONE");
        result.ToString().Should().NotContain("IN_PROGRESS");
    }

}