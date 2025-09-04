using TaskManagamentSystem.Api.DTOs;

namespace TaskManagamentSystem.Api.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserResponse>> GetAllUsersAsync();
}