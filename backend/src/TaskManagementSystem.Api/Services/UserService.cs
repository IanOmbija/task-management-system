using Microsoft.EntityFrameworkCore;
using TaskManagamentSystem.Api.Data;
using TaskManagamentSystem.Api.DTOs;

namespace TaskManagamentSystem.Api.Services;

public class UserService : Services.Interfaces.IUserService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<UserService> _logger;
    public UserService(ApplicationDbContext dbContext, ILogger<UserService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<IEnumerable<UserResponse>> GetAllUsersAsync()
    {
        try
        {
            var allUsers = await _dbContext.Users
                .AsNoTracking()
                .Select(u => new UserResponse
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    Role = u.Role.ToString()
                })
                .ToListAsync();

            //check for available users, if non - log response
            if (allUsers.Count == 0)
                _logger.LogInformation(" No Users found in the database");

            return allUsers;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error: There was a problem fetching users from the database");
            throw;
        }
    }
}