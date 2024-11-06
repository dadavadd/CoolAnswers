using CoolAnswers.DTOs.User;
using CoolAnswers.Models;
using CoolAnswers.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoolAnswers.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IJwtService _jwtService;

    public AuthController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IJwtService jwtService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = new User
        {
            UserName = registerDto.Username,
            Email = registerDto.Email,
            Point = 0,
            Rank = "Новичок"
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "User");
            return Ok(new { Message = "Регистрация прошла успешно. Вы можете войти." });
        }


        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userManager.FindByNameAsync(loginDto.Username);

        if (user == null)
            return BadRequest("Неверный логин или пароль.");

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (result.Succeeded)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user, roles);

            Response.Cookies.Append("userkey", token, new()
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(4)
            });

            return Ok(new { token, user.UserName, user.Rank, user.Point });
        }

        return BadRequest("Неверный логин или пароль.");
    }
}
