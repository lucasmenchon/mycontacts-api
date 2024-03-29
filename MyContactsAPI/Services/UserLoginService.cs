
using ContactsManage.Data;
using MyContactsAPI.Dtos.User;
using MyContactsAPI.Interfaces;
using MyContactsAPI.Models;
using MyContactsAPI.Models.EmailModels;
using MyContactsAPI.Models.LoginModels;
using MyContactsAPI.Models.UserModels;

namespace MyContactsAPI.Services
{
    public class UserLoginService : IUserLoginService
    {
        private readonly IUserRepository _userRepository;

        public UserLoginService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<LoginApiResponse> UserSigIn(UserSignInDto userSignIn)
        {
            try
            {
                try
                {
                    var email = new Email(userSignIn.Email);
                }
                catch
                {
                    return new LoginApiResponse($"Email inválido.", 400);
                }

                User? user = await _userRepository.GetUserByEmailAsync(userSignIn.Email);
                if (user is null)
                    return new LoginApiResponse("Perfil não encontrado", 404);

                if (!user.Password.Challenge(userSignIn.Password))
                    return new LoginApiResponse("Usuário ou senha inválidos", 400);

                try
                {
                    if (!user.Email.Verification.IsActive)
                        return new LoginApiResponse("Conta inativa", 400);
                }
                catch
                {
                    return new LoginApiResponse("Não foi possível verificar seu perfil", 500);
                }

                try
                {
                    var data = new Models.LoginModels.ResponseData
                    {
                        Token = JwtTokenService.GenerateToken(user)
                    };

                    return new LoginApiResponse(string.Empty, data);
                }
                catch
                {
                    return new LoginApiResponse("Não foi possível obter os dados do perfil", 500);
                }
            }
            catch
            {
                return new LoginApiResponse("Não foi possível encontrar seu perfil", 500);
            }
        }
    }
}
