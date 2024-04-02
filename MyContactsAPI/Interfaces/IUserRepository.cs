using MyContactsAPI.Dtos.User;
using MyContactsAPI.Models;
using MyContactsAPI.Models.UserModels;

namespace MyContactsAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<ApiResponse> CreateUserAsync(CreateUserDto createUserDto);
        Task<ApiResponse> UpdateUserAsync(UpdateUserDto userUpdateDto);        
        Task<ApiResponse> DeleteUserAsync();
    }
}
