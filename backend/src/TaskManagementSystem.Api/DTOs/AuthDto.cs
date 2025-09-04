using System.ComponentModel.DataAnnotations;

namespace TaskManagamentSystem.Api.DTOs;

public class RegisterUser
{
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}

public class LoginUser
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}

public class AuthResponse
{
    public string Username { get; set; } = default!;
    public string Token { get; set; } = default!;
    public string Role { get; set; } = default!;
    public DateTime ExpiresAt { get; set; }
    public Guid UserId { get; set; }

    public bool Success { get; set; }
    public string Message { get; set; }
    
}