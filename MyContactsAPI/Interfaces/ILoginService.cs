using MyContactsAPI.Dtos.User;
using MyContactsAPI.Models;

namespace MyContactsAPI.Interfaces
{
    public interface ILoginService
    {
        Task<ApiResponse> UserSigIn(UserSignInDto userSignIn);
    }
}
