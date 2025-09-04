using Microsoft.Extensions.Logging;
using TaskManagamentSystem.Api.DTOs;
using TaskManagamentSystem.Api.Models;
using TaskManagamentSystem.Api.Models.Enums;
using TaskManagamentSystem.Api.Services.Interfaces;
using TaskManagamentSystem.Api.Helpers;
using TaskManagamentSystem.Api.Security;
using TaskManagamentSystem.Api.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace TaskManagamentSystem.Api.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly JwtTokenService _jwtTokenService;
    private readonly PasswordHasher<User> _passwordHasher;
    private readonly ILogger<AuthService> _looger;

    public AuthService(ApplicationDbContext dbContext, JwtTokenService jwtTokenService, ILogger<AuthService> logger)
    {
        _dbContext = dbContext;
        _jwtTokenService = jwtTokenService;
        _passwordHasher = new PasswordHasher<User>();
        _looger = logger;
    }

    public async Task<AuthResponse> RegisterUser(RegisterUser request)
    {
        try
        {
            var existingUser = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username);
                
            if (existingUser != null)
            {
                _looger.LogWarning("Registration Failed: Username  - {Username} already exists.", request.Username);
                return AuthHelper.Fail("Registration Failed: Username already exists.");
            }

            var user = new User
            {
                Username = request.Username.Trim(),
                Email = request.Email.Trim().ToLowerInvariant(),
                Role = Role.USER
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            _looger.LogInformation("Success: New User - {Email} registered sucessfully.", user.Email);

            var jwt = _jwtTokenService.GenerateToken(user);
            return AuthHelper.Build(user, jwt, "New User registered sucessfully." );
            
        }
        catch (Exception ex)
        {
            _looger.LogError(ex, "Error: There was an error registering user {Email}", request.Email);
            return AuthHelper.Fail("Error: An error occured while registering a user");
        }
    }

    public async Task<AuthResponse> LoginUser(LoginUser request)
    {
        try
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync
                (x => x.Email == request.Email);
            if (user == null)
            {
                _looger.LogWarning("Login failed: User with Email {Email} not found.", request.Email);
                //throw new UnauthorizedAccessException("Email not found!");
                return AuthHelper.Fail("Login Failed: User not found");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                _looger.LogWarning("Login failed: Invalid password");
                //throw new Exception("Invalid Password");
                return AuthHelper.Fail("Login Failed: Invalid Password in use.");
            }

            _looger.LogInformation("User {Username} logged in successfully.", user.Email);
            var jwt = _jwtTokenService.GenerateToken(user);
            return AuthHelper.Build(user, jwt, "User successfully logged in.");
        }
        catch (Exception ex)
        {
            _looger.LogError(ex, "Error: Login error in user {Username}", request.Email);
            return AuthHelper.Fail("Error: Login Failed!");
        }
    }
}