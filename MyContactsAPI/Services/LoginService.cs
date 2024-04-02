
using ContactsManage.Data;
using MyContactsAPI.Dtos.User;
using MyContactsAPI.Interfaces;
using MyContactsAPI.Models;
using MyContactsAPI.Models.EmailModels;
using MyContactsAPI.Models.UserModels;
using MyContactsAPI.SharedContext;

namespace MyContactsAPI.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUserRepository _userRepository;

        public LoginService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ApiResponse> UserSigIn(UserSignInDto userSignIn)
        {
            try
            {
                try
                {
                    var email = new Email(userSignIn.Email);
                }
                catch
                {
                    return new ApiResponse($"Email inválido.", 400);
                }

                User? user = await _userRepository.GetUserByEmailAsync(userSignIn.Email);
                if (user is null)
                    return new ApiResponse("Perfil não encontrado", 404);

                if (!user.Password.Challenge(userSignIn.Password))
                    return new ApiResponse("Usuário ou senha inválidos", 400);

                try
                {
                    if (!user.Email.Verification.IsActive)
                        return new ApiResponse("Conta inativa", 400);
                }
                catch
                {
                    return new ApiResponse("Não foi possível verificar seu perfil", 500);
                }

                try
                {
                    var data = new ResponseData
                    {
                        Token = JwtTokenService.GenerateToken(user)
                    };

                    return new ApiResponse(string.Empty, data);
                }
                catch
                {
                    return new ApiResponse("Não foi possível obter os dados do perfil", 500);
                }
            }
            catch
            {
                return new ApiResponse("Não foi possível encontrar seu perfil", 500);
            }
        }
    }
}
