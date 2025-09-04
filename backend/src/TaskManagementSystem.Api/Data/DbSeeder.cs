using TaskManagamentSystem.Api.Models;
using TaskManagamentSystem.Api.Models.Enums;
using TaskManagamentSystem.Api.Services.Interfaces;
using Task = System.Threading.Tasks.Task;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace TaskManagamentSystem.Api.Data;

public static class DbSeeder
{

    public static async Task SeedAsync(ApplicationDbContext dbContext, ILogger logger)
    {
        var passwordHasher = new PasswordHasher<User>();

        if (dbContext.Users.Any())
        {
            logger.LogInformation("Skipping initialization, database already seeded.");
            return;
        }

        logger.LogInformation("Starting database seeeding process...");

        // admin user creation on db
        var adminUser = new User
        {
            Username = "admin",
            Email = "admin@example.com",
            Role = Role.ADMIN
        };
        adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Admin@123");

        // normal user creation on db
        var normalUser = new User
        {
            Username = "user",
            Email = "user@example.com",
            Role = Role.USER
        };
        normalUser.PasswordHash = passwordHasher.HashPassword(normalUser, "User@123");

        dbContext.Users.AddRange(adminUser, normalUser);
        await dbContext.SaveChangesAsync();
        logger.LogInformation("Default users seeded: {Admin}, {User}", adminUser.Username, normalUser.Username);

        // Sample tasks creation on the db

        var tasks = Enumerable.Range(1, 6).Select(i => new TaskManagamentSystem.Api.Models.Task
        {
            Tittle = $"Test Task {i}",
            Description = "This is a sample task seeded",
            taskPriority = i % 3 == 0 ? TaskPriority.HIGH :
                            i % 2 == 0 ? TaskPriority.MEDIUM :
                                            TaskPriority.LOW,

            CreatorId = adminUser.Id,
            AssigneeId = i % 2 == 0 ? normalUser.Id : adminUser.Id,
            Status = i % 3 == 0 ? Models.Enums.TaskStatus.IN_PROGRESS :
                     i % 5 == 0 ? Models.Enums.TaskStatus.DONE :
                                    Models.Enums.TaskStatus.TODO
        }).ToList();

        dbContext.Tasks.AddRange(tasks);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("{Count} Tasks seeded.", tasks.Count);
        logger.LogInformation("---Completed: Database seeded done ---");
    }
}