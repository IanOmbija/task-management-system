using TaskManagamentSystem.Api.DTOs;

namespace TaskManagamentSystem.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterUser(RegisterUser request);
        Task<AuthResponse> LoginUser(LoginUser request);
    }
}