using MyContactsAPI.Dtos.User;
using MyContactsAPI.Models.LoginModels;

namespace MyContactsAPI.Interfaces
{
    public interface IUserLoginService
    {
        Task<LoginResponse> UserSigIn(UserSignInDto userSignIn);
    }
}
