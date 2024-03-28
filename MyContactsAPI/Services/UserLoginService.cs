
using ContactsManage.Data;
using MyContactsAPI.Dtos.User;
using MyContactsAPI.Interfaces;
using MyContactsAPI.Models;
using MyContactsAPI.Models.LoginModels;
using MyContactsAPI.Models.UserModels;

namespace MyContactsAPI.Services
{
    public class UserLoginService : IUserLoginService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;

        public UserLoginService(IJwtTokenService jwtTokenService, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<LoginResponse> UserSigIn(UserSignInDto userSignIn)
        {
            try
            {
                // Verifique se o email fornecido é válido
                try
                {
                    // Instancie um objeto Email com o endereço de email fornecido
                    var email = new Email(userSignIn.Email);
                }
                catch
                {
                    return new LoginResponse($"Email inválido.", 400); // Retorna uma resposta de erro 400 Bad Request se o email não for válido
                }

                // Se o email for válido, continue com o restante da lógica de login
                User? user = await _userRepository.GetUserByEmailAsync(userSignIn.Email);
                if (user is null)
                    return new LoginResponse("Perfil não encontrado", 404);

                if (!user.Password.Challenge(userSignIn.Password))
                    return new LoginResponse("Usuário ou senha inválidos", 400);

                try
                {
                    if (!user.Email.Verification.IsActive)
                        return new LoginResponse("Conta inativa", 400);
                }
                catch
                {
                    return new LoginResponse("Não foi possível verificar seu perfil", 500);
                }

                try
                {

                    var token = _jwtTokenService.GenerateToken(user);

                    var data = new Models.LoginModels.ResponseData
                    {
                        Token = token
                    };


                    return new LoginResponse(string.Empty, data);
                }
                catch
                {
                    return new LoginResponse("Não foi possível obter os dados do perfil", 500);
                }
            }
            catch
            {
                return new LoginResponse("Não foi possível encontrar seu perfil", 500);
            }
        }
    }
}
