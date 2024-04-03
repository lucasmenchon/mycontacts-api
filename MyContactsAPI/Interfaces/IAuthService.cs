using MyContactsAPI.Dtos.User;
using MyContactsAPI.Models;

namespace MyContactsAPI.Interfaces;

public interface IAuthService
{
    Task<ApiResponse> UserSigIn(UserSignInDto userSignIn);
    Task<ApiResponse> CreateUserAsync(CreateUserDto createUserDto);
}

