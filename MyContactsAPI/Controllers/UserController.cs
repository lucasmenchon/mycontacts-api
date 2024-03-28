using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyContactsAPI.Dtos.User;
using MyContactsAPI.Interfaces;
using MyContactsAPI.Models;
using System.Security.Claims;
using System.Threading;

namespace MyContactsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;

        public UserController(IJwtTokenService jwtTokenService, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
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
        
        [HttpPut("UpdateUser")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto userUpdate)
        {
            try
            {
                // Agora você pode usar o userId para identificar o usuário autenticado
                var response = await _userRepository.UpdateUserAsync(userUpdate);
                return NoContent();
            }
            catch (Exception error)
            {
                return StatusCode(500, $"Ops!! Não foi possível atualizar seu usuário, tente novamente ou entre em contato com o suporte, detalhes do erro: {error.Message}");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser()
        {
            try
            {
                // Seu código para excluir o usuário

                return NoContent();
            }
            catch (Exception error)
            {
                return StatusCode(500, $"Ops!! Não foi possível apagar seu usuário, tente novamente ou entre em contato com o suporte, detalhes do erro: {error.Message}");
            }
        }
    }
}
