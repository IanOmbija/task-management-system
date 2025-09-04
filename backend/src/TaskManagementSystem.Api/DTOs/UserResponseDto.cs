namespace TaskManagamentSystem.Api.DTOs;

public class UserResponse
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; } = default!;
    public string Role { get; set; } = default!;
}