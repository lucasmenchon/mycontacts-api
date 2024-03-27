using Microsoft.AspNetCore.Mvc;
using MyContactsAPI.Interfaces;
using MyContactsAPI.Models;
using MyContactsAPI.Services;

namespace MyContactsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _email;
        private readonly IUserPasswordService _userPasswordService;
        private readonly IJwtTokenService _jwtTokenService;

        public LoginController(IUserRepository userRepository, IEmailService email, IJwtTokenService jwtTokenService, IUserPasswordService userPasswordService)
        {
            _userRepository = userRepository;
            _email = email;
            _jwtTokenService = jwtTokenService;
            _userPasswordService = userPasswordService;
        }

        [HttpPost]
        public async Task<IActionResult> LoginAccess(SignIn sigIn)
        {
            if (ModelState.IsValid)
            {
                User user = await _userRepository.FindByEmailAsync(sigIn.Email);

                if (user != null && await _userPasswordService.CheckPasswordAsync(user, sigIn.Password))
                {
                    //await _session.CreateSessionAsync(user);
                    string token = _jwtTokenService.GenerateToken(user);

                    return Ok($"Bearer {token}");
                    //return Ok(new { token });
                }
                else
                {
                    return Unauthorized("Nome de usuário ou senha inválidos.");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
