using AuthenticationAPI.Controllers;
using AuthenticationCL.DTOs;
using AuthenticationCL.IServices;
using AuthenticationCL.Services;
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
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var user = await _authService.Register(dto);
        return Ok(new { message = "User registered", email = user.Email });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var token = await _authService.Login(dto);
        return Ok(new { token });
    }

}
