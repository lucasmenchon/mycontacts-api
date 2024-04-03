using Microsoft.AspNetCore.Mvc;
using MyContactsAPI.Dtos.User;
using MyContactsAPI.Interfaces;
using MyContactsAPI.Models.UserModels;
using MyContactsAPI.Repositories;
using Newtonsoft.Json.Linq;

namespace MyContactsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IAuthService _authService;

        public AuthenticationController(IAuthService userLogin)
        {
            _authService = userLogin;
        }

        [HttpPost, Route("Login")]
        public async Task<IActionResult> LoginAccess(UserSignInDto userSigIn)
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

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody] CreateUserDto createUserDto)
        {
            try
            {
                var response = await _authService.CreateUserAsync(createUserDto);
                return Ok(response);
            }
            catch (Exception error)
            {
                return StatusCode(500, $"Oops!! Unable to register user, try again or contact support, error details: {error.Message}");
            }
        }

    }
}
