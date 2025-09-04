using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskManagamentSystem.Api.Services.Interfaces;

namespace TaskManagamentSystem.Api.Controllers;

[ApiController]
[Route("api/users")]
//[Authorize(Roles = "USER")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserService userService, ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsersAsync()
    {
        var result = await _userService.GetAllUsersAsync();
        return Ok(result);
    }
}