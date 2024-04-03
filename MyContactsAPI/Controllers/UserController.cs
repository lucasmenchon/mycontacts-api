using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyContactsAPI.Dtos.User;
using MyContactsAPI.Interfaces;

namespace MyContactsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [Authorize]
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto userUpdateDto)
        {
            try
            {
                var response = await _userRepository.UpdateUserAsync(userUpdateDto);
                return Ok(response);
            }
            catch (Exception error)
            {
                return StatusCode(500, $"Oops!! Unable to update your user, please try again or contact support, error details: {error.Message}");
            }
        }

        [Authorize]
        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser()
        {
            try
            {
                var response = await _userRepository.DeleteUserAsync();
                return Ok(response);
            }
            catch (Exception error)
            {
                return StatusCode(500, $"Oops!! Unable to delete your username, try again or contact support, error details: {error.Message}");
            }
        }
    }
}
