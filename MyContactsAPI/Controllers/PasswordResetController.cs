using Microsoft.AspNetCore.Mvc;
using MyContactsAPI.Interfaces;
using MyContactsAPI.Models;
using MyContactsAPI.Services;
using MyContactsAPI.ViewModels;

namespace MyContactsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PasswordResetController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserPasswordService _userPasswordService;
        private readonly JwtTokenService _jwtTokenService;
        private readonly IEmailService _email;

        public PasswordResetController(IUserRepository userRepository, IUserPasswordService userPasswordService, JwtTokenService jwtTokenService, IEmailService email)
        {
            _userRepository = userRepository;
            _userPasswordService = userPasswordService;
            _jwtTokenService = jwtTokenService;
            _email = email;
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(PasswordResetViewModel passwordReset)
        {
            try
            {
                Guid userId = _jwtTokenService.ValidateJwtToken(passwordReset.Token);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Obtém o usuário logado
                //UserViewModel userLogged = _session.FindUserSession();
                UserViewModel userLogged = new UserViewModel() { Id = userId };
                passwordReset.Id = userId;
                // Verifica se a senha atual está correta
                bool isPasswordCorrect = await _userPasswordService.CheckChangedPasswordAsync(userLogged, passwordReset.OldPassword);
                if (!isPasswordCorrect)
                {
                    return BadRequest("A senha atual está incorreta.");
                }

                // Reseta a senha do usuário
                await _userPasswordService.ResetPasswordAsync(passwordReset);

                return Ok();
            }
            catch (Exception error)
            {
                return StatusCode(500, $"Ops!! Não conseguimos alterar sua senha. Detalhe do erro: {error.Message}");
            }
        }

        [HttpPost("SendPasswordResetEmail")]
        public async Task<IActionResult> SendPasswordResetEmail(string email)
        {
            if (ModelState.IsValid)
            {
                // Busca o usuário pelo e-mail
                User user = await _userRepository.FindByEmailAsync(email);

                if (user != null)
                {
                    // Gera o token JWT com prazo de validade de 1 hora
                    var token = _jwtTokenService.GenerateToken(user);

                    // Constrói o link de redefinição de senha com o token JWT
                    var resetLink = Url.Action("ResetPassword", "Account", new { token }, Request.Scheme);

                    // Constrói a mensagem de e-mail
                    string subject = "Redefinição de Senha";
                    string message = $"Para redefinir sua senha, clique no link a seguir: <a href='{resetLink}'>Redefinir Senha</a>";

                    // Envia o e-mail com o link de redefinição de senha
                    //await _email.SendVerificationEmailAsync(email, subject, message);

                    return Ok("Um e-mail com instruções para redefinir sua senha foi enviado para o seu endereço de e-mail.");
                }
                else
                {
                    return NotFound("Usuário não encontrado.");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
