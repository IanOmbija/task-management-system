using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TaskManagamentSystem.Api.DTOs;
using TaskManagamentSystem.Api.Services.Interfaces;

namespace TaskManagamentSystem.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUser request)
    {
        if (ModelState.IsValid)
        {
            var result = await _authService.RegisterUser(request);
            return Ok(result);
        }
        else
        {
            return BadRequest(ModelState);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUser request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.LoginUser(request);
        return Ok(result);
    }
}