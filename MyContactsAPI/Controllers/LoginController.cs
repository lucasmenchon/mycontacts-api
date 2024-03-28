using Microsoft.AspNetCore.Mvc;
using MyContactsAPI.Dtos.User;
using MyContactsAPI.Interfaces;
using MyContactsAPI.Models.UserModels;

namespace MyContactsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly IUserLoginService _loginService;
        private readonly IEmailService _email;
        private readonly IUserPasswordService _userPasswordService;
        private readonly IJwtTokenService _jwtTokenService;

        public LoginController(IUserLoginService userLogin, IEmailService email, IJwtTokenService jwtTokenService, IUserPasswordService userPasswordService)
        {
            _loginService = userLogin;
            _email = email;
            _jwtTokenService = jwtTokenService;
            _userPasswordService = userPasswordService;
        }

        [HttpPost]
        public async Task<IActionResult> LoginAccess(UserSignInDto userSigIn)
        {
            if (ModelState.IsValid)
            {
                var response = await _loginService.UserSigIn(userSigIn);

                return Ok(response);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
