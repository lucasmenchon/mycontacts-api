using MyContactsAPI.Dtos.User;
using MyContactsAPI.Models;

namespace MyContactsAPI.Interfaces;

public interface IAuthRepository
{
    Task<ApiResponse> UserSigIn(UserSignInDto userSignIn);
    Task<ApiResponse> CreateUserAsync(CreateUserDto createUserDto);
}