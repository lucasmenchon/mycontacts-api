using Microsoft.AspNetCore.Mvc;
using MyContactsAPI.Dtos.User;
using MyContactsAPI.Interfaces;
using MyContactsAPI.Models.UserModels;

namespace MyContactsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IUserLoginService _loginService;

        public AuthenticationController(IUserLoginService userLogin)
        {
            _loginService = userLogin;
        }

        [HttpPost, Route("Login")]
        public async Task<IActionResult> LoginAccess(UserSignInDto userSigIn)
        {
            var response = await _loginService.UserSigIn(userSigIn);
            return Ok(response);
        }
    }
}
