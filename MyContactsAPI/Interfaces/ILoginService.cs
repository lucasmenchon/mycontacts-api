using MyContactsAPI.Dtos.User;
using MyContactsAPI.Models.LoginModels;

namespace MyContactsAPI.Interfaces
{
    public interface ILoginService
    {
        Task<LoginApiResponse> UserSigIn(UserSignInDto userSignIn);
    }
}
