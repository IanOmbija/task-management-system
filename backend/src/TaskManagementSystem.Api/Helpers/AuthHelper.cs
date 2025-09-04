namespace TaskManagamentSystem.Api.Helpers;

using TaskManagamentSystem.Api.DTOs;
using TaskManagamentSystem.Api.Models;

public static class AuthHelper
{
    /// <summary>
    /// Builds an AuthResponse DTO from a user
    /// </summary>
    /// <param name="user"></param>
    /// <param name="jwt"></param>
    /// <returns></returns>
    public static AuthResponse Build(User user, (string token, DateTime expiresAt) jwt, string message = "Success")
    => new()
    {
        Success = true,
        Message = message,
        Token = jwt.token,
        ExpiresAt = jwt.expiresAt,
        Role = user.Role.ToString(),
        Username = user.Username,
        UserId = user.Id
    };

    /// <summary>
    /// Builds a failed response
    /// </summary>
    public static AuthResponse Fail(string message)
    => new()
    {
        Success = false,
        Message = message
    };
}