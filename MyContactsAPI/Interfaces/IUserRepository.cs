using MyContactsAPI.Dtos.User;
using MyContactsAPI.Models.UserModels;

namespace MyContactsAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<UserApiResponse> CreateUserAsync(CreateUserDto createUserDto);
        Task<UserApiResponse> UpdateUserAsync(UpdateUserDto userUpdateDto);        
        Task<UserApiResponse> DeleteUserAsync();
    }
}
