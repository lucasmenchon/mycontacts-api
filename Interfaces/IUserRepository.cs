using MyContactsAPI.Dtos.User;
using MyContactsAPI.Models;
using MyContactsAPI.Models.UserModels;

namespace MyContactsAPI.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByEmailAsync(string email);
    Task<ApiResponse> UpdateUserAsync(UpdateUserDto userUpdateDto);
    Task<ApiResponse> DeleteUserAsync();
}
