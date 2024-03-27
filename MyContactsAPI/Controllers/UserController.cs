using Microsoft.AspNetCore.Mvc;
using MyContactsAPI.Dtos;
using MyContactsAPI.Interfaces;
using MyContactsAPI.Models;
using MyContactsAPI.ViewModels;
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

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            try
            {
                var response = await _userRepository.CreateUserAsync(createUserDto);
                return Ok(response.Message);
            }
            catch (Exception error)
            {
                return StatusCode(500, $"Ops!! Não foi possível cadastrar o usuário, tente novamente ou entre em contato com o suporte, detalhes do erro: {error.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserViewModel userUpdate)
        {
            try
            {
                // Seu código para atualizar o usuário

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
