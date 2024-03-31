using Microsoft.AspNetCore.Mvc;
using MyContactsAPI.Dtos.User;
using MyContactsAPI.Interfaces;
using MyContactsAPI.Models.UserModels;
using Newtonsoft.Json.Linq;

namespace MyContactsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly ILoginService _loginService;

        public AuthenticationController(ILoginService userLogin)
        {
            _loginService = userLogin;
        }

        [HttpPost, Route("Login")]
        public async Task<IActionResult> LoginAccess(UserSignInDto userSigIn)
        {
            var response = await _loginService.UserSigIn(userSigIn);

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
    }
}
