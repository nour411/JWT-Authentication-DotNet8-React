using AuthenticationAPI.Controllers;
using AuthenticationCL.DTOs;
using AuthenticationCL.IServices;
using AuthenticationCL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class CommuneController : BaseController
{
    private readonly IAuthService _authService;

    public CommuneController(IAuthService authService)
    {
        _authService = authService;
    }
 
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterRequestDTO request)
    {
        var result = await _authService.RegisterAsync(request);
        return Ok(result);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LogInRequestDTO request)
    {
        var result = await _authService.LoginAsync(request);
        return Ok(result);
    }
    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public IActionResult Admin()
    {
        return Ok("Welcome Admin!");
    }

    [HttpGet("user")]
    [Authorize(Roles = "User,Admin")]
    public IActionResult User()
    {
        return Ok("Welcome User!");
    }

}
