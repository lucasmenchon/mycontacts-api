using Microsoft.AspNetCore.Mvc;
using MyContactsAPI.Dtos.User;
using MyContactsAPI.Interfaces;

namespace MyContactsAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : Controller
{
    private readonly IAuthRepository _authService;

    public AuthenticationController(IAuthRepository userLogin)
    {
        _authService = userLogin;
    }

    [HttpGet, Route("Test")]
    public async Task<IActionResult> Test()
    {
        var response = "test";
        return Ok(response);
    }

    [HttpPost, Route("Login")]
    public async Task<IActionResult> LoginAccess(UserSignInDto userSigIn)
    {
        try
        {
            var response = await _authService.UserSigIn(userSigIn);

            if (response.Data != null)
            {
                Response.Cookies.Append("AuthToken", $"Bearer {response.Data.Token}", new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTimeOffset.UtcNow.AddHours(2),
                    Secure = true
                });
            }

            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Oops!! Unable to login, try again or contact support, error details: {ex.Message}");
        }
    }

    [HttpPost("RegisterUser")]
    public async Task<IActionResult> RegisterUser([FromBody] CreateUserDto createUserDto)
    {
        try
        {
            var response = await _authService.CreateUserAsync(createUserDto);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Oops!! Unable to register user, try again or contact support, error details: {ex.Message}");
        }
    }
}
