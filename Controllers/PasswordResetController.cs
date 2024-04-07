using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyContactsAPI.Dtos.Password;
using MyContactsAPI.Interfaces;

namespace MyContactsAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PasswordResetController : Controller
{
    private readonly IUserPasswordRepository _userPasswordService;

    public PasswordResetController(IUserPasswordRepository userPasswordService)
    {
        _userPasswordService = userPasswordService;
    }

    [Authorize]
    [HttpPost("ChangePassword")]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto passwordToChange)
    {
        try
        {
            var response = await _userPasswordService.ChangeUserPassword(passwordToChange);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Oops!! Unable to change your password, please try again or contact support, error details: {ex.Message}");
        }
    }

    [HttpPost("SendPasswordResetEmail")]
    public async Task<IActionResult> SendPasswordResetEmail(string email)
    {
        try
        {
            var response = await _userPasswordService.SendPasswordResetEmail(email);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Oops!! Unable to send email, please try again or contact support, error details: {ex.Message}");
        }
    }
}

