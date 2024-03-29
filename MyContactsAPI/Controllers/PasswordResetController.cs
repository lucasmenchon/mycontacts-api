using Microsoft.AspNetCore.Mvc;
using MyContactsAPI.Interfaces;
using MyContactsAPI.Models.UserModels;
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
        private readonly IEmailService _email;

        public PasswordResetController(IUserRepository userRepository, IUserPasswordService userPasswordService, IEmailService email)
        {
            _userRepository = userRepository;
            _userPasswordService = userPasswordService;
            _email = email;
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(PasswordResetViewModel passwordReset)
        {
            //try
            //{
            //    // Verifica se a senha atual está correta
            //    bool isPasswordCorrect = await _userPasswordService.CheckPasswordAsync(userLogged, passwordReset.OldPassword);
            //    if (!isPasswordCorrect)
            //    {
            //        return BadRequest("A senha atual está incorreta.");
            //    }

            //    // Reseta a senha do usuário
            //    await _userPasswordService.ResetPasswordAsync(passwordReset);

            //    return Ok();
            //}
            //catch (Exception error)
            //{
            //    return StatusCode(500, $"Ops!! Não conseguimos alterar sua senha. Detalhe do erro: {error.Message}");
            //}
            return Ok();
        }

        [HttpPost("SendPasswordResetEmail")]
        public async Task<IActionResult> SendPasswordResetEmail(string email)
        {
            if (ModelState.IsValid)
            {
                // Busca o usuário pelo email
                User user = new User("");

                if (user != null)
                {
                    // Gera o token JWT com prazo de validade de 1 hora
                    var token = JwtTokenService.GenerateToken(user);

                    // Constrói o link de redefinição de senha com o token JWT
                    var resetLink = Url.Action("ResetPassword", "Account", new { token }, Request.Scheme);

                    // Constrói a mensagem de email
                    string subject = "Redefinição de Senha";
                    string message = $"Para redefinir sua senha, clique no link a seguir: <a href='{resetLink}'>Redefinir Senha</a>";

                    // Envia o email com o link de redefinição de senha
                    //await _email.SendVerificationEmailAsync(email, subject, message);

                    return Ok("Um email com instruções para redefinir sua senha foi enviado para o seu endereço de email.");
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
