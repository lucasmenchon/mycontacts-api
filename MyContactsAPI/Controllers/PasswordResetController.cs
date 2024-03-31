using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyContactsAPI.Dtos.Password;
using MyContactsAPI.Interfaces;
using MyContactsAPI.Models.UserModels;
using MyContactsAPI.Services;

namespace MyContactsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PasswordResetController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserPasswordService _userPasswordService;

        public PasswordResetController(IUserRepository userRepository, IUserPasswordService userPasswordService)
        {
            _userRepository = userRepository;
            _userPasswordService = userPasswordService;
        }

        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto passwordToChange)
        {
            var response = await _userPasswordService.ChangeUserPassword(passwordToChange);

            return Ok(response);
        }

        [HttpPost("SendPasswordResetEmail")]
        public async Task<IActionResult> SendPasswordResetEmail(string email)
        {
            var response =  await _userPasswordService.SendPasswordResetEmail(email);
            return Ok(response);
        }
    }
}
