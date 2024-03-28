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

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            try
            {
                var response = await _userRepository.CreateUserAsync(createUserDto);
                return Ok(response);
            }
            catch (Exception error)
            {
                return StatusCode(500, $"Ops!! Não foi possível cadastrar o usuário, tente novamente ou entre em contato com o suporte, detalhes do erro: {error.Message}");
            }
        }

        [HttpPut("UpdateUser"), Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto userUpdateDto)
        {
            try
            {
                var response = await _userRepository.UpdateUserAsync(userUpdateDto);
                return Ok(response);
            }
            catch (Exception error)
            {
                return StatusCode(500, $"Ops!! Não foi possível atualizar seu usuário, tente novamente ou entre em contato com o suporte, detalhes do erro: {error.Message}");
            }
        }

        [HttpDelete("DeleteUser"), Authorize]
        public async Task<IActionResult> DeleteUser()
        {
            try
            {
                var response = await _userRepository.DeleteUserAsync();
                return Ok(response);
            }
            catch (Exception error)
            {
                return StatusCode(500, $"Ops!! Não foi possível apagar seu usuário, tente novamente ou entre em contato com o suporte, detalhes do erro: {error.Message}");
            }
        }
    }
}
